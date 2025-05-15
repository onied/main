import 'package:onied_mobile/models/course_card_model.dart';
import 'package:onied_mobile/repositories/course_repository.dart';

abstract class CatalogBlocState {
  final String query;
  final CoursesFilterPredicate filteringPredicate;

  CatalogBlocState({required this.query, required this.filteringPredicate});
}

class ErrorState extends CatalogBlocState {
  final String errorMessage;

  ErrorState({
    required this.errorMessage,
    required super.query,
    required super.filteringPredicate,
  });
}

class LoadingState extends CatalogBlocState {
  LoadingState({required super.query, required super.filteringPredicate});
}

class LoadedState extends CatalogBlocState {
  late final List<CourseCardModel> searchResults;
  final Iterable<String> categories;

  LoadedState({
    required this.categories,
    required this.searchResults,
    required super.query,
    required super.filteringPredicate,
  });
}
