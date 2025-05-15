import 'package:onied_mobile/models/course_preview_model.dart';

abstract class CoursePreviewBlocState {}

class LoadingState extends CoursePreviewBlocState {}

class ErrorState extends CoursePreviewBlocState {
  String errorMessage;

  ErrorState({required this.errorMessage});
}

class LoadedState extends CoursePreviewBlocState {
  final CoursePreviewModel course;

  LoadedState({required this.course});
}
