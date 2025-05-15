import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/models/course_card_model.dart';
import 'package:onied_mobile/repositories/course_repository.dart';

import 'catalog_bloc_event.dart';
import 'catalog_bloc_state.dart';

class CatalogBloc extends Bloc<CatalogBlocEvent, CatalogBlocState> {
  final CourseRepository courseRepository;

  CatalogBloc({required this.courseRepository})
    : super(
        LoadingState(
          query: "",
          filteringPredicate: (CourseCardModel courseCardDto) => true,
        ),
      ) {
    on<InitializeCourses>(_onInitializeCourses);
    on<UpdateSearchQuery>(_onUpdateSearchQuery);
    on<UpdateFilterPredicate>(_onUpdateFilterPredicate);
  }

  Future<void> _onInitializeCourses(
    InitializeCourses event,
    Emitter<CatalogBlocState> emit,
  ) async {
    emit(
      LoadingState(
        query: state.query,
        filteringPredicate: state.filteringPredicate,
      ),
    );
    final results = (await courseRepository.getAllCourses()).toList();
    final categories = (await courseRepository.getAllCategories()).toList();
    emit(
      LoadedState(
        categories: categories,
        query: state.query,
        filteringPredicate: state.filteringPredicate,
        searchResults: results,
      ),
    );
  }

  Future<void> _onUpdateSearchQuery(
    UpdateSearchQuery event,
    Emitter<CatalogBlocState> emit,
  ) async {
    if (state is! LoadedState) return;
    final currentState = state as LoadedState;
    emit(
      LoadingState(
        query: event.query,
        filteringPredicate: state.filteringPredicate,
      ),
    );

    final results =
        (await courseRepository.getFilteredCourses(
          state.query,
          state.filteringPredicate,
        )).toList();

    emit(
      LoadedState(
        categories: currentState.categories,
        query: state.query,
        filteringPredicate: state.filteringPredicate,
        searchResults: results,
      ),
    );
  }

  Future<void> _onUpdateFilterPredicate(
    UpdateFilterPredicate event,
    Emitter<CatalogBlocState> emit,
  ) async {
    if (state is! LoadedState) return;
    final currentState = state as LoadedState;
    emit(
      LoadingState(
        query: state.query,
        filteringPredicate: state.filteringPredicate,
      ),
    );

    final results =
        (await courseRepository.getFilteredCourses(
          state.query,
          state.filteringPredicate,
        )).toList();

    emit(
      LoadedState(
        categories: currentState.categories,
        query: state.query,
        filteringPredicate: state.filteringPredicate,
        searchResults: results,
      ),
    );
  }
}
