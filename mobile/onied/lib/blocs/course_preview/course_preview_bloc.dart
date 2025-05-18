import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/repositories/course_repository.dart';

import 'course_preview_bloc_event.dart';
import 'course_preview_bloc_state.dart';

class CoursePreviewBloc
    extends Bloc<CoursePreviewBlocEvent, CoursePreviewBlocState> {
  final CourseRepository courseRepository;

  CoursePreviewBloc({required this.courseRepository}) : super(LoadingState()) {
    on<LoadCoursePreview>((event, emit) async {
      emit(LoadingState());
      try {
        final course = await courseRepository.getCourseById(event.courseId);
        if (course != null) {
          emit(LoadedState(course: course));
        } else {
          emit(ErrorState(errorMessage: "Course not found."));
        }
      } catch (e) {
        emit(ErrorState(errorMessage: "Failed to load course"));
      }
    });
  }
}
