import 'package:design_courses/data/course_card_dto.dart';
import 'package:design_courses/data/repository_mock.dart';
import 'package:design_courses/widgets/catalog/search_mode_app_bar.dart';
import 'package:flutter/material.dart';
import 'package:design_courses/widgets/catalog/course_card.dart';

class SearchResultsPage extends StatelessWidget {
  final List<CourseCardDto> filteredCourses =
      MockPreviewRepository().getAllCourses();

  SearchResultsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const SearchModeAppBar(),
      body: ListView.builder(
        itemCount: filteredCourses.length,
        itemBuilder: (context, index) {
          final course = filteredCourses[index];
          return CourseCard(courseCardDto: course);
        },
      ),
    );
  }
}
