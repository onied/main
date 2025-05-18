import 'package:onied_mobile/models/course_block_model.dart';
import 'package:onied_mobile/models/course_hierarchy_model.dart';
import 'package:onied_mobile/models/course_mini_card_model.dart';
import 'package:onied_mobile/models/course_preview_model.dart';
import 'package:onied_mobile/models/course_card_model.dart';
import 'package:onied_mobile/providers/courses_provider.dart';

typedef CoursesFilterPredicate = bool Function(CourseCardModel courseCardDto);

class CourseRepository {
  final CourseProvider _courseProvider;
  CourseRepository(this._courseProvider);

  Future<Iterable<String>> getAllCategories() async {
    final queryResult = await _courseProvider.getAllCategories();
    if (queryResult.hasException) {
      throw Exception(
        'Error fetching categories: ${queryResult.exception.toString()}',
      );
    }
    final data = queryResult.data;
    if (data == null || data['categories'] == null) {
      return <String>[];
    }
    final rawList = data['categories'] as List<dynamic>;
    final categoriesNames =
        rawList
            .map((entry) => entry?['name'] as String?)
            .whereType<String>()
            .toList();

    return categoriesNames;
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
      throw Exception('No course data received for id $courseId');
    }

    return CoursePreviewModel.fromJson(json);
  }

  Future<Iterable<CourseCardModel>> getAllCourses() async {
    throw UnimplementedError("");
  }

  Future<Iterable<CourseCardModel>> getFilteredCourses(
    String searchQuery,
    CoursesFilterPredicate filterPredicate,
  ) async {
    return (await getAllCourses())
        .where((course) => course.title.contains(searchQuery))
        .where(filterPredicate)
        .toList();
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
    throw UnimplementedError("");
  }

  Future<CourseBlockModel?> getCourseBlockById(int blockId) async {
    throw UnimplementedError("");
  }
}
