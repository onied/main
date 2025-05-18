import 'package:onied_mobile/models/course_card_model.dart';
import 'package:onied_mobile/models/course_preview_model.dart';
import 'package:onied_mobile/models/search_filters_model.dart';
import 'package:onied_mobile/repositories/course_repository.dart';

abstract class CatalogBlocState {
  final String query;
  final SearchFiltersModel searchFilters;

  CatalogBlocState({required this.query, required this.searchFilters});
}

class ErrorState extends CatalogBlocState {
  final String errorMessage;

  ErrorState({
    required this.errorMessage,
    required super.query,
    required super.searchFilters,
  });
}

class LoadingState extends CatalogBlocState {
  LoadingState({required super.query, required super.searchFilters});
}

class LoadedState extends CatalogBlocState {
  late final List<CourseCardModel> searchResults;
  final Iterable<CategoryModel> categories;

  LoadedState({
    required this.categories,
    required this.searchResults,
    required super.query,
    required super.searchFilters,
  });
}
