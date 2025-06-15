import 'dart:async';

import 'package:dart_amqp/dart_amqp.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/app/config.dart';
import 'package:onied_mobile/app/injection.dart';
import 'package:onied_mobile/blocs/course_preview/course_preview_bloc.dart';
import 'package:onied_mobile/blocs/course_preview/course_preview_bloc_event.dart';
import 'package:onied_mobile/blocs/course_preview/course_preview_bloc_state.dart';
import 'package:onied_mobile/components/button/button.dart';
import 'package:onied_mobile/components/picture_preview/picture_preview.dart';
import 'package:onied_mobile/components/search_bar/search_bar.dart';
import 'package:onied_mobile/models/course_preview_model.dart';
import 'package:onied_mobile/providers/courses_provider.dart';
import 'package:onied_mobile/repositories/course_repository.dart';
import 'components/allow_certificates.dart';
import 'components/author_block.dart';
import 'components/course_program.dart';
import 'package:flutter/material.dart';

enum OpenedTab { description, stats }

Future<Consumer> _initConsumer() async {
  final client = Client(
    settings: ConnectionSettings(
      host: Config.rabbitMqHost,
      port: 5672,
      authProvider: const PlainAuthenticator('user', 'useruser'),
    ),
  );
  final channel = await client.channel();

  // Создаем очередь
  final queue = await channel.queue('onied-stats-queue', durable: true);
  // Создаем потребителя, с тегом my_consumer
  final consumer = await queue.consume(consumerTag: 'mobile');
  return consumer;
}

class CoursePreviewPage extends StatefulWidget {
  final String courseId;

  const CoursePreviewPage({super.key, required this.courseId});

  @override
  State<CoursePreviewPage> createState() => _CoursePreviewPageState();
}

class _CoursePreviewPageState extends State<CoursePreviewPage> {
  late final StreamSubscription streamSubscription;
  Consumer? consumer;

  @override
  initState() {
    super.initState();
    _initConsumer().then((cons) {
      setState(() {
        consumer = cons;
        streamSubscription = cons.listen((AmqpMessage amqpMessage) {
          final bloc = context.read<CoursePreviewBloc>();
          if (bloc.state is! StatsOpenState) {
            amqpMessage.reject(true);
            return;
          }
          final courseId = (bloc.state as StatsOpenState).course.id;
          if (amqpMessage.payloadAsJson["courseId"] != courseId) {
            amqpMessage.reject(true);
            return;
          }
          bloc.add(UpdateStats(amqpMessage.payloadAsJson["likes"]));
        });
      });
    });
  }

  @override
  void dispose() {
    streamSubscription.cancel();
    consumer?.cancel();
    super.dispose();
  }

  Widget buildHeader(
    CoursePreviewModel course,
    OpenedTab openedTab,
    CoursePreviewBloc bloc,
  ) {
    return Column(
      children: [
        Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            PreviewPicture(
              width: 120,
              height: 180,
              href: course.pictureHref,
              isArchived: course.isArchived,
            ),
            const SizedBox(width: 16),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    course.title,
                    style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                  ),
                  SizedBox(height: 4),
                  AuthorBlock(
                    authorName: course.courseAuthor.name,
                    authorAvatarHref: course.courseAuthor.avatarHref,
                  ),
                  SizedBox(height: 4),
                  Text(course.category.name, style: TextStyle(fontSize: 14)),
                  course.hasCertificates
                      ? AllowCertificate()
                      : SizedBox.shrink(),
                ],
              ),
            ),
          ],
        ),
        SegmentedButton(
          segments: [
            ButtonSegment(
              value: OpenedTab.description,
              label: Text("Описание"),
              icon: Icon(Icons.description),
            ),
            ButtonSegment(
              value: OpenedTab.stats,
              label: Text("Статистика"),
              icon: Icon(Icons.analytics),
            ),
          ],
          selected: {openedTab},
          onSelectionChanged: (Set<OpenedTab> newSelection) {
            if (newSelection.first == OpenedTab.description) {
              bloc.add(CloseStats());
            } else {
              bloc.add(OpenStats());
            }
          },
        ),
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) => CoursePreviewBloc(
            courseRepository: CourseRepository(getIt<CourseProvider>()),
          )..add(LoadCoursePreview(widget.courseId)),
      child: Scaffold(
        appBar: const CourseSearchBar(),
        body: BlocBuilder<CoursePreviewBloc, CoursePreviewBlocState>(
          builder: (context, state) {
            return switch (state) {
              LoadingState() => const Center(
                child: CircularProgressIndicator(),
              ),
              ErrorState(:final errorMessage) => Center(
                child: Text(errorMessage),
              ),
              LoadedState(:final course) => SingleChildScrollView(
                child: Padding(
                  padding: const EdgeInsets.all(16),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      buildHeader(
                        course,
                        OpenedTab.description,
                        context.read<CoursePreviewBloc>(),
                      ),
                      Container(
                        width: double.infinity,
                        padding: EdgeInsets.symmetric(vertical: 16),
                        child: Column(
                          children: [
                            Button(
                              text: course.isOwned ? "продолжить" : "купить",
                              onPressed: () {
                                if (course.isOwned) {
                                  context.push("/course/${course.id}/learn");
                                } else {
                                  context.push("/purchase");
                                }
                              },
                            ),
                            Button(
                              text:
                                  course.isLiked
                                      ? "убрать из понравившихся"
                                      : "в понравившиеся",
                              onPressed: () {
                                context.read<CoursePreviewBloc>().add(
                                  LikeCurrentCourse(!course.isLiked),
                                );
                              },
                            ),
                          ],
                        ),
                      ),
                      // Description
                      Text(
                        course.description,
                        style: TextStyle(fontSize: 16, height: 1.5),
                      ),
                      Padding(padding: EdgeInsets.only(bottom: 10)),
                      CourseProgram(modules: course.courseProgram),
                    ],
                  ),
                ),
              ),
              StatsOpenState(:final course, :final likes) =>
                SingleChildScrollView(
                  child: Padding(
                    padding: const EdgeInsets.all(16),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        buildHeader(
                          course,
                          OpenedTab.stats,
                          context.read<CoursePreviewBloc>(),
                        ),
                        Text("В понравившихся: $likes"),
                      ],
                    ),
                  ),
                ),
              _ => const Center(child: Text("Something went wrong.")),
            };
          },
        ),
      ),
    );
  }
}
