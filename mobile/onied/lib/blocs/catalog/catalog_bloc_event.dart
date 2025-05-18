import 'package:onied_mobile/repositories/course_repository.dart';

abstract class CatalogBlocEvent {}

class InitializeCourses extends CatalogBlocEvent {}

class UpdateSearchQuery extends CatalogBlocEvent {
  final String query;

  UpdateSearchQuery(this.query);
}

class UpdateFilterPredicate extends CatalogBlocEvent {
  final CoursesFilterPredicate predicate;

  UpdateFilterPredicate(this.predicate);
}
