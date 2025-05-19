import 'package:onied_mobile/models/course_block_model.dart';
import 'package:onied_mobile/models/course_hierarchy_model.dart';
import 'package:onied_mobile/models/course_mini_card_model.dart';
import 'package:onied_mobile/models/course_preview_model.dart';
import 'package:onied_mobile/models/course_card_model.dart';
import 'package:onied_mobile/models/enums/block_type.dart';
import 'package:onied_mobile/models/search_filters_model.dart';
import 'package:onied_mobile/providers/courses_provider.dart';

class CourseRepository {
  final CourseProvider _courseProvider;
  CourseRepository(this._courseProvider);

  Future<Iterable<CategoryModel>> getAllCategories() async {
    final queryResult = await _courseProvider.getAllCategories();
    if (queryResult.hasException) {
      throw Exception(
        'Error fetching categories: ${queryResult.exception.toString()}',
      );
    }
    final data = queryResult.data;
    if (data == null || data['categories'] == null) {
      return <CategoryModel>[];
    }
    final rawList = data['categories'] as List<dynamic>;
    final categories =
        rawList
            .map((categoryJson) => CategoryModel.fromJson(categoryJson))
            .toList()
          ..add(CategoryModel(id: -1, name: "All"));

    return categories;
  }

  Future<CoursePreviewModel?> getCourseById(int courseId) async {
    final queryResult = await _courseProvider.getCoursePreviewById(courseId);

    if (queryResult.hasException) {
      throw Exception(
        'Error fetching course preview: ${queryResult.exception.toString()}',
      );
    }

    final data = queryResult.data;
    final json = data?['publicCourseById'] as Map<String, dynamic>?;
    if (json == null) {
      throw Exception('No course data received for course with id $courseId');
    }

    return CoursePreviewModel.fromJson(json);
  }

  Future<Iterable<CourseCardModel>> getFilteredCourses(
    String searchQuery,
    SearchFiltersModel searchFilters,
  ) async {
    final queryResult = await _courseProvider.getSearchResult(
      searchQuery,
      searchFilters,
    );

    if (queryResult.hasException) {
      throw Exception(
        'Error fetching catalog: ${queryResult.exception.toString()}',
      );
    }

    final data = queryResult.data;
    if (data == null ||
        data['courses'] == null ||
        data['courses']['nodes'] == null) {
      return <CourseCardModel>[];
    }

    final rawList = data['courses']['nodes'] as List<dynamic>;
    final results = rawList.map((node) {
      final json = node as Map<String, dynamic>;

      return CourseCardModel.fromJson(json);
    });

    return results;
  }

  Future<Iterable<CourseMiniCardModel>> getOwnedCourses() async {
    final queryResult = await _courseProvider.getOwnedCourses(25);

    if (queryResult.hasException) {
      throw Exception(
        'Error fetching owned courses: ${queryResult.exception.toString()}',
      );
    }

    final data = queryResult.data;
    if (data == null ||
        data['ownedCourses'] == null ||
        data['ownedCourses']['nodes'] == null) {
      return <CourseMiniCardModel>[];
    }

    final rawList = data['ownedCourses']['nodes'] as List<dynamic>;

    final courses = rawList.map((node) {
      final json = node as Map<String, dynamic>;

      return CourseMiniCardModel.fromJson(json);
    });

    return courses;
  }

  Future<Iterable<CourseMiniCardModel>> getPopularCourses() async {
    final queryResult = await _courseProvider.getPopularCourses(25);

    if (queryResult.hasException) {
      throw Exception(
        'Error fetching popular courses: ${queryResult.exception.toString()}',
      );
    }

    final data = queryResult.data;
    if (data == null ||
        data['popularCourses'] == null ||
        data['popularCourses']['nodes'] == null) {
      return <CourseMiniCardModel>[];
    }

    final rawList = data['popularCourses']['nodes'] as List<dynamic>;

    final courses = rawList.map((node) {
      final json = node as Map<String, dynamic>;

      return CourseMiniCardModel.fromJson(json);
    });

    return courses;
  }

  Future<Iterable<CourseMiniCardModel>> getRecommendedCourses() async {
    final queryResult = await _courseProvider.getRecommendedCourses(25);

    if (queryResult.hasException) {
      throw Exception(
        'Error fetching recommended courses: ${queryResult.exception.toString()}',
      );
    }

    final data = queryResult.data;
    if (data == null ||
        data['courses'] == null ||
        data['courses']['nodes'] == null) {
      return <CourseMiniCardModel>[];
    }

    final rawList = data['courses']['nodes'] as List<dynamic>;

    final courses = rawList.map((node) {
      final json = node as Map<String, dynamic>;

      return CourseMiniCardModel.fromJson(json);
    });

    return courses;
  }

  Future<CourseHierarchyModel?> getCourseHierarchy(int courseId) async {
    final queryResult = await _courseProvider.getCourseHierarchyById(courseId);

    if (queryResult.hasException) {
      throw Exception(
        'Error fetching course hierarchy: ${queryResult.exception.toString()}',
      );
    }

    final data = queryResult.data;
    final json = data?['courseById'] as Map<String, dynamic>?;
    if (json == null) {
      throw Exception(
        'No hierarchy data received for course with id $courseId',
      );
    }

    return CourseHierarchyModel.fromJson(json);
  }

  Future<CourseBlockModel?> getCourseBlockById(
    int blockId,
    BlockType blockType,
  ) async {
    final queryResult = await switch (blockType) {
      BlockType.summaryBlock => _courseProvider.getSummaryBlockById(blockId),
      BlockType.videoBlock => _courseProvider.getVideoBlockById(blockId),
      BlockType.tasksBlock => _courseProvider.getTasksBlockById(blockId),
      BlockType.anyBlock =>
        throw UnimplementedError("Can't get block without specified type"),
    };

    if (queryResult.hasException) {
      throw Exception(
        'Error fetching block with id: ${queryResult.exception.toString()}',
      );
    }

    final data = queryResult.data;
    final json =
        data?[graphqlDataFieldFromBlockType(blockType)]
            as Map<String, dynamic>?;
    if (json == null) {
      throw Exception('No hierarchy data received for course with id $blockId');
    }

    switch (blockType) {
      case BlockType.summaryBlock:
        return CourseSummaryBlockModel.fromJson(json);
      case BlockType.videoBlock:
        return CourseVideoBlockModel.fromJson(json);
      case BlockType.tasksBlock:
        return CourseTaskBlockModel.fromJson(json);
      default:
        throw UnimplementedError("Can't get block without specified type");
    }
  }
}
