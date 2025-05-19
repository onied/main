import 'package:onied_mobile/models/course_mini_card_model.dart';

abstract class HomeBlocState {}

class LoadingState extends HomeBlocState {}

class ErrorState extends HomeBlocState {
  String errorMessage;

  ErrorState({required this.errorMessage});
}

class LoadedState extends HomeBlocState {
  final Iterable<CourseMiniCardModel> ownedCourses;
  final Iterable<CourseMiniCardModel> popularCourses;
  final Iterable<CourseMiniCardModel> recommendedCourses;

  LoadedState({
    required this.ownedCourses,
    required this.popularCourses,
    required this.recommendedCourses,
  });

  LoadedState copyWith({
    Iterable<CourseMiniCardModel>? ownedCourses,
    Iterable<CourseMiniCardModel>? popularCourses,
    Iterable<CourseMiniCardModel>? recommendedCourses,
  }) {
    return LoadedState(
      ownedCourses: ownedCourses ?? this.ownedCourses,
      popularCourses: popularCourses ?? this.popularCourses,
      recommendedCourses: recommendedCourses ?? this.recommendedCourses,
    );
  }
}
