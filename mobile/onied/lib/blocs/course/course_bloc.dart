import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/repositories/course_repository.dart';

import 'course_bloc_event.dart';
import 'course_bloc_state.dart';

class CourseBloc extends Bloc<CourseBlocEvent, CourseBlocState> {
  final CourseRepository courseRepository;

  CourseBloc({required this.courseRepository}) : super(LoadingState()) {
    on<LoadHierarchy>(_onLoadHierarchy);
    on<LoadBlock>(_onLoadBlock);
  }

  Future<void> _onLoadHierarchy(
    LoadHierarchy event,
    Emitter<CourseBlocState> emit,
  ) async {
    emit(LoadingState());
    final hierarchy = await courseRepository.getCourseHierarchy(event.courseId);
    if (hierarchy == null) {
      emit(ErrorState(errorMessage: "Could not find course."));
    } else {
      emit(LoadedState(hierarchy: hierarchy, isBlockLoading: false));
    }
  }

  Future<void> _onLoadBlock(
    LoadBlock event,
    Emitter<CourseBlocState> emit,
  ) async {
    if (state is! LoadedState) return;
    final currentState = state as LoadedState;
    emit(currentState.copyWith(isBlockLoading: true));
    final block = await courseRepository.getCourseBlockById(event.blockId);
    if (block == null) {
      emit(ErrorState(errorMessage: "Could not find course block."));
    } else {
      emit(currentState.copyWith(isBlockLoading: false, block: block));
    }
  }
}
