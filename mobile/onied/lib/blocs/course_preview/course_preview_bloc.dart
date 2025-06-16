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
        final course = await courseRepository.getCourseById(
          int.parse(event.courseId),
        );
        if (course != null) {
          emit(LoadedState(course: course));
        } else {
          emit(ErrorState(errorMessage: "Course not found."));
        }
      } catch (e) {
        emit(ErrorState(errorMessage: "Failed to load course, reason: ${e}"));
      }
    });
    on<LikeCurrentCourse>((event, emit) async {
      if (state is! LoadedState) return;
      final currentState = state as LoadedState;
      final success = await courseRepository.likeCourse(
        currentState.course.id,
        event.like,
      );
      emit(
        LoadedState(
          course: currentState.course.copyWith(
            isLiked: currentState.course.isLiked ^ success,
          ),
        ),
      );
    });
    on<OpenStats>((event, emit) async {
      if (state is! LoadedState) return;
      final currentState = state as LoadedState;
      emit(StatsOpenState(course: currentState.course, likes: null));
    });
    on<CloseStats>((event, emit) async {
      if (state is! StatsOpenState) return;
      final currentState = state as StatsOpenState;
      emit(LoadedState(course: currentState.course));
    });
    on<UpdateStats>((event, emit) async {
      if (state is! StatsOpenState) return;
      final currentState = state as StatsOpenState;
      emit(StatsOpenState(course: currentState.course, likes: event.likes));
    });
  }
}
