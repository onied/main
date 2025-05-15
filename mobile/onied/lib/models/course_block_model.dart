import 'package:onied_mobile/models/enums/block_type.dart';
import 'package:onied_mobile/models/enums/task_type.dart';

class CourseBlockModel {
  final int id;
  final String title;
  final BlockType blockType;
  final int index;

  CourseBlockModel({
    required this.id,
    required this.title,
    required this.blockType,
    required this.index,
  });
}

class CourseConspectBlockModel extends CourseBlockModel {
  final String markdownText;
  final String? fileName;
  final String? fileHref;

  CourseConspectBlockModel({
    required super.id,
    required super.title,
    required super.index,
    required this.markdownText,
    this.fileName,
    this.fileHref,
  }) : super(blockType: BlockType.summaryBlock);
}

class CourseVideoBlockModel extends CourseBlockModel {
  final String href;

  CourseVideoBlockModel({
    required super.id,
    required super.title,
    required super.index,
    required this.href,
  }) : super(blockType: BlockType.videoBlock);
}

class CourseTaskBlockModel extends CourseBlockModel {
  final List<CourseTaskBlockTask> tasks;

  CourseTaskBlockModel({
    required super.id,
    required super.title,
    required super.index,
    required this.tasks,
  }) : super(blockType: BlockType.tasksBlock);
}

class CourseTaskBlockTask {
  final int id;
  final String title;
  final bool isDone;
  final TaskType taskType;
  final int maxPoints;
  final List<CourseTaskBlockTaskVariant>? variants;

  CourseTaskBlockTask({
    required this.id,
    required this.title,
    required this.isDone,
    required this.taskType,
    required this.maxPoints,
    this.variants,
  });
}

class CourseTaskBlockTaskVariant {
  final int id;
  final String description;

  CourseTaskBlockTaskVariant({required this.id, required this.description});
}
