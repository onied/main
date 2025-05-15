abstract class CourseBlocEvent {}

class LoadHierarchy extends CourseBlocEvent {
  final int courseId;

  LoadHierarchy({required this.courseId});
}

class LoadBlock extends CourseBlocEvent {
  final int blockId;

  LoadBlock({required this.blockId});
}
