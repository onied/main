import 'package:onied_mobile/models/enums/block_type.dart';

abstract class CourseBlocEvent {}

class LoadHierarchy extends CourseBlocEvent {
  final int courseId;

  LoadHierarchy({required this.courseId});
}

class LoadBlock extends CourseBlocEvent {
  final int blockId;
  final BlockType blockType;

  LoadBlock({required this.blockId, required this.blockType});
}
