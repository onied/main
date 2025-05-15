import 'package:onied_mobile/models/course_block_model.dart';
import 'package:onied_mobile/models/course_hierarchy_model.dart';

abstract class CourseBlocState {}

class LoadingState extends CourseBlocState {}

class ErrorState extends CourseBlocState {
  String errorMessage;
  ErrorState({required this.errorMessage});
}

class LoadedState extends CourseBlocState {
  final CourseHierarchyModel hierarchy;
  final CourseBlockModel? block;
  final bool isBlockLoading;

  LoadedState({
    required this.hierarchy,
    required this.isBlockLoading,
    this.block,
  });

  LoadedState copyWith({
    CourseHierarchyModel? hierarchy,
    CourseBlockModel? block,
    bool? isBlockLoading,
    bool? isSidebarVisible,
  }) {
    return LoadedState(
      hierarchy: hierarchy ?? this.hierarchy,
      block: block ?? this.block,
      isBlockLoading: isBlockLoading ?? this.isBlockLoading,
    );
  }
}
