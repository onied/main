import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/repositories/course_repository.dart';

import 'home_bloc_event.dart';
import 'home_bloc_state.dart';

class HomeBloc extends Bloc<HomeBlocEvent, HomeBlocState> {
  final CourseRepository repository;

  HomeBloc({required this.repository}) : super(LoadingState()) {
    on<LoadCourses>(_onLoadCourses);
  }

  Future<void> _onLoadCourses(
    LoadCourses event,
    Emitter<HomeBlocState> emit,
  ) async {
    try {
      final ownedCourses = await repository.getOwnedCourses();
      final popularCourses = await repository.getPopularCourses();
      final recommendedCourses = await repository.getRecommendedCourses();

      emit(
        LoadedState(
          ownedCourses: ownedCourses,
          popularCourses: popularCourses,
          recommendedCourses: recommendedCourses,
        ),
      );
    } catch (e) {
      emit(ErrorState(errorMessage: "Could not load main page."));
    }
  }
}
