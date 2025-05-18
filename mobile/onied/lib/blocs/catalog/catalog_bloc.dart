import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/models/course_preview_model.dart';
import 'package:onied_mobile/models/search_filters_model.dart';
import 'package:onied_mobile/repositories/course_repository.dart';

import 'catalog_bloc_event.dart';
import 'catalog_bloc_state.dart';

class CatalogBloc extends Bloc<CatalogBlocEvent, CatalogBlocState> {
  final CourseRepository courseRepository;

  CatalogBloc({required this.courseRepository})
    : super(
        LoadingState(
          query: "",
          searchFilters: SearchFiltersModel(
            selectedCategory: CategoryModel(id: -1, name: "All"),
            selectedPriceRange: RangeValues(0, 50_000),
            selectedCompletionTimeRange: RangeValues(0, 150),
            selectedHasCertificates: true,
            selectedIsArchived: false,
          ),
        ),
      ) {
    on<InitializeCourses>(_onInitializeCourses);
    on<UpdateSearchQuery>(_onUpdateSearchQuery);
    on<UpdateSearchFilters>(_onUpdateSearchFilters);
  }

  Future<void> _onInitializeCourses(
    InitializeCourses event,
    Emitter<CatalogBlocState> emit,
  ) async {
    emit(LoadingState(query: state.query, searchFilters: state.searchFilters));
    final results =
        (await courseRepository.getFilteredCourses(
          state.query,
          state.searchFilters,
        )).toList();
    final categories = (await courseRepository.getAllCategories()).toList();
    emit(
      LoadedState(
        categories: categories,
        query: state.query,
        searchFilters: state.searchFilters,
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
    emit(LoadingState(query: event.query, searchFilters: state.searchFilters));

    final results =
        (await courseRepository.getFilteredCourses(
          state.query,
          state.searchFilters,
        )).toList();

    emit(
      LoadedState(
        categories: currentState.categories,
        query: state.query,
        searchFilters: state.searchFilters,
        searchResults: results,
      ),
    );
  }

  Future<void> _onUpdateSearchFilters(
    UpdateSearchFilters event,
    Emitter<CatalogBlocState> emit,
  ) async {
    if (state is! LoadedState) return;
    final currentState = state as LoadedState;
    emit(LoadingState(query: state.query, searchFilters: event.searchFilters));

    final results =
        (await courseRepository.getFilteredCourses(
          state.query,
          state.searchFilters,
        )).toList();

    emit(
      LoadedState(
        categories: currentState.categories,
        query: state.query,
        searchFilters: state.searchFilters,
        searchResults: results,
      ),
    );
  }
}
