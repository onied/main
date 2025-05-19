import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/app/injection.dart';
import 'package:onied_mobile/blocs/home/home_bloc.dart';
import 'package:onied_mobile/blocs/home/home_bloc_event.dart';
import 'package:onied_mobile/blocs/home/home_bloc_state.dart';
import 'package:onied_mobile/components/search_bar/search_bar.dart';
import 'package:onied_mobile/models/course_mini_card_model.dart';
import 'package:onied_mobile/providers/courses_provider.dart';
import 'package:onied_mobile/repositories/course_repository.dart';

class HomePage extends StatelessWidget {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) =>
              HomeBloc(repository: CourseRepository(getIt<CourseProvider>()))
                ..add(LoadCourses()),
      child: Scaffold(
        appBar: CourseSearchBar(),
        body: BlocBuilder<HomeBloc, HomeBlocState>(
          builder: (context, state) {
            return switch (state) {
              LoadingState() => const Center(
                child: CircularProgressIndicator(),
              ),
              ErrorState(:final errorMessage) => Center(
                child: Text(errorMessage),
              ),
              LoadedState(
                :final ownedCourses,
                :final popularCourses,
                :final recommendedCourses,
              ) =>
                ListView(
                  children: [
                    buildCourseSection(context, 'Мои курсы', ownedCourses),
                    buildCourseSection(
                      context,
                      'Популярные курсы',
                      popularCourses,
                    ),
                    buildCourseSection(
                      context,
                      'Рекомендуемые курсы',
                      recommendedCourses,
                    ),
                  ],
                ),
              _ => const Center(child: Text('Something went wrong.')),
            };
          },
        ),
      ),
    );
  }

  Widget buildCourseSection(
    BuildContext context,
    String title,
    Iterable<CourseMiniCardModel> courses,
  ) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16.0, vertical: 16.0),
          child: Text(
            title,
            style: const TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
          ),
        ),
        SizedBox(
          height: 150,
          child: SingleChildScrollView(
            scrollDirection: Axis.horizontal,
            padding: const EdgeInsets.symmetric(horizontal: 8.0),
            child: Row(
              children:
                  courses.map((course) {
                    return Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 8.0),
                      child: InkWell(
                        onTap: () => context.push("/course/${course.id}"),
                        child: Stack(
                          alignment: Alignment.bottomLeft,
                          children: [
                            Container(
                              width: 100,
                              height: 140,
                              decoration: BoxDecoration(
                                border: Border.all(color: Colors.black),
                                borderRadius: BorderRadius.circular(8),
                                image: DecorationImage(
                                  image: NetworkImage(course.imageUrl),
                                  fit: BoxFit.cover,
                                ),
                              ),
                            ),
                            Container(
                              width: 100,
                              color: Colors.black.withAlpha(128),
                              padding: const EdgeInsets.symmetric(
                                horizontal: 4,
                                vertical: 2,
                              ),
                              child: Text(
                                course.title,
                                style: TextStyle(
                                  color: Colors.white,
                                  fontSize: 12,
                                  fontWeight: FontWeight.w500,
                                ),
                                textAlign: TextAlign.center,
                              ),
                            ),
                          ],
                        ),
                      ),
                    );
                  }).toList(),
            ),
          ),
        ),
      ],
    );
  }
}
