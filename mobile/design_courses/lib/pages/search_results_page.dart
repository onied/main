import 'package:design_courses/data/course_card_dto.dart';
import 'package:design_courses/data/repository_mock.dart';
import 'package:design_courses/widgets/catalog/search_filters.dart';
import 'package:design_courses/widgets/catalog/search_mode_app_bar.dart';
import 'package:flutter/material.dart';
import 'package:design_courses/widgets/shared/course_card.dart';

class SearchResultsPage extends StatefulWidget {
  const SearchResultsPage({super.key});

  @override
  State<StatefulWidget> createState() => _SearchResultsPageState();
}

class _SearchResultsPageState extends State<SearchResultsPage> {
  String query = '';
  CoursesFilterPredicate filteringPredicate =
      (CourseCardDto courseCardDto) => true;
  List<CourseCardDto> searchResults = MockPreviewRepository().getAllCourses();

  void updateSearch(String newQuery) {
    setState(() {
      query = newQuery;
      searchResults = MockPreviewRepository().getFilteredCourses(
        query,
        filteringPredicate,
      );
    });
  }

  void updateFilters(CoursesFilterPredicate newFilteringPredicate) {
    setState(() {
      filteringPredicate = newFilteringPredicate;
      searchResults = MockPreviewRepository().getFilteredCourses(
        query,
        filteringPredicate,
      );
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: SearchModeAppBar(
        onSearchChanged: updateSearch,
        onFiltersPredicateChanged: updateFilters,
      ),
      body: ListView.builder(
        itemCount: searchResults.length,
        itemBuilder: (context, index) {
          final course = searchResults[index];
          return CourseCard(courseCardDto: course);
        },
      ),
    );
  }
}
