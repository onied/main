import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/blocs/course_preview/course_preview_bloc.dart';
import 'package:onied_mobile/blocs/course_preview/course_preview_bloc_event.dart';
import 'package:onied_mobile/blocs/course_preview/course_preview_bloc_state.dart';
import 'package:onied_mobile/components/button/button.dart';
import 'package:onied_mobile/components/picture_preview/picture_preview.dart';
import 'package:onied_mobile/components/search_bar/search_bar.dart';
import 'package:onied_mobile/repositories/course_repository.dart';
import 'components/allow_certificates.dart';
import 'components/author_block.dart';
import 'components/course_program.dart';
import 'package:flutter/material.dart';

class CoursePreviewPage extends StatelessWidget {
  final String courseId;

  const CoursePreviewPage({super.key, required this.courseId});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) =>
              CoursePreviewBloc(courseRepository: CourseRepository())
                ..add(LoadCoursePreview(courseId)),
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
              LoadedState(:final course) => Padding(
                padding: const EdgeInsets.all(16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
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
                                style: TextStyle(
                                  fontSize: 18,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                              SizedBox(height: 4),
                              AuthorBlock(
                                authorName: course.courseAuthor.name,
                                authorAvatarHref:
                                    course.courseAuthor.avatarHref,
                              ),
                              SizedBox(height: 4),
                              Text(
                                course.category.name,
                                style: TextStyle(fontSize: 14),
                              ),
                              course.hasCertificates
                                  ? AllowCertificate()
                                  : SizedBox.shrink(),
                            ],
                          ),
                        ),
                      ],
                    ),
                    Container(
                      width: double.infinity,
                      padding: EdgeInsets.symmetric(vertical: 16),
                      child: Button(
                        text: course.isOwned ? "продолжить" : "купить",
                        onPressed: () {
                          if (course.isOwned) {
                            context.push("/course/${course.id}/learn");
                          } else {
                            context.push("/purchase");
                          }
                        },
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
              _ => const Center(child: Text("Something went wrong.")),
            };
          },
        ),
      ),
    );
  }
}
