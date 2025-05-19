import 'package:onied_mobile/models/search_filters_model.dart';

abstract class CatalogBlocEvent {}

class InitializeCourses extends CatalogBlocEvent {}

class UpdateSearchQuery extends CatalogBlocEvent {
  final String query;

  UpdateSearchQuery(this.query);
}

class UpdateSearchFilters extends CatalogBlocEvent {
  final SearchFiltersModel searchFilters;

  UpdateSearchFilters(this.searchFilters);
}
