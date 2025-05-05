import 'package:design_courses/data/repository_mock.dart';
import 'package:design_courses/pages/course_preview_page.dart';
import 'package:design_courses/widgets/shared/app_bar.dart';
import 'package:flutter/material.dart';
import 'package:design_courses/data/course_card_dto.dart';

class HomePage extends StatelessWidget {
  const HomePage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: CustomAppBar(),
      body: ListView(
        children: [
          buildCourseSection(
            context,
            'Мои курсы',
            MockPreviewRepository().getOwnedCourses(),
          ),
          buildCourseSection(
            context,
            'Популярные курсы',
            MockPreviewRepository().getPopularCourses(),
          ),
          buildCourseSection(
            context,
            'Рекомендуемые курсы',
            MockPreviewRepository().getRecommendedCourses(),
          ),
        ],
      ),
    );
  }

  Widget buildCourseSection(
    BuildContext context,
    String title,
    List<CourseCardDto> courses,
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
                        onTap:
                            () => Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder:
                                    (context) =>
                                        PreviewPage(courseId: course.id),
                              ),
                            ),
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
