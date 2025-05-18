import 'package:onied_mobile/models/course_card_model.dart';

abstract class HomeBlocState {}

class LoadingState extends HomeBlocState {}

class ErrorState extends HomeBlocState {
  String errorMessage;

  ErrorState({required this.errorMessage});
}

class LoadedState extends HomeBlocState {
  final Iterable<CourseCardModel> ownedCourses;
  final Iterable<CourseCardModel> popularCourses;
  final Iterable<CourseCardModel> recommendedCourses;

  LoadedState({
    required this.ownedCourses,
    required this.popularCourses,
    required this.recommendedCourses,
  });

  LoadedState copyWith({
    Iterable<CourseCardModel>? ownedCourses,
    Iterable<CourseCardModel>? popularCourses,
    Iterable<CourseCardModel>? recommendedCourses,
  }) {
    return LoadedState(
      ownedCourses: ownedCourses ?? this.ownedCourses,
      popularCourses: popularCourses ?? this.popularCourses,
      recommendedCourses: recommendedCourses ?? this.recommendedCourses,
    );
  }
}
