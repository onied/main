import 'package:onied_mobile/models/enums/block_type.dart';
import 'package:onied_mobile/models/enums/task_type.dart';

class CourseBlockModel {
  final int id;
  final String title;
  final BlockType blockType;

  CourseBlockModel({
    required this.id,
    required this.title,
    required this.blockType,
  });

  factory CourseBlockModel.fromJson(
    Map<String, dynamic> json,
    BlockType blockType,
  ) {
    return CourseBlockModel(
      id: json["id"] as int,
      title: json["title"] as String,
      blockType: blockType,
    );
  }
}

class CourseSummaryBlockModel extends CourseBlockModel {
  final String markdownText;
  final String? fileName;
  final String? fileHref;

  CourseSummaryBlockModel({
    required super.id,
    required super.title,
    required this.markdownText,
    this.fileName,
    this.fileHref,
  }) : super(blockType: BlockType.summaryBlock);

  factory CourseSummaryBlockModel.fromJson(Map<String, dynamic> json) {
    return CourseSummaryBlockModel(
      id: json["id"] as int,
      title: json["title"],
      markdownText: json["markdownText"] as String,
      fileName: json["fileName"] as String?,
      fileHref: json["fileHref"] as String?,
    );
  }
}

class CourseVideoBlockModel extends CourseBlockModel {
  final String href;

  CourseVideoBlockModel({
    required super.id,
    required super.title,
    required this.href,
  }) : super(blockType: BlockType.videoBlock);

  factory CourseVideoBlockModel.fromJson(Map<String, dynamic> json) {
    return CourseVideoBlockModel(
      id: json["id"] as int,
      title: json["title"],
      href: json["href"] as String,
    );
  }
}

class CourseTaskBlockModel extends CourseBlockModel {
  final List<CourseTaskBlockTask> tasks;

  CourseTaskBlockModel({
    required super.id,
    required super.title,
    required this.tasks,
  }) : super(blockType: BlockType.tasksBlock);

  factory CourseTaskBlockModel.fromJson(Map<String, dynamic> json) {
    return CourseTaskBlockModel(
      id: json["id"] as int,
      title: json["title"],
      tasks:
          (json["tasks"] as List)
              .map((taskJson) => CourseTaskBlockTask.fromJson(taskJson))
              .toList(),
    );
  }
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

  factory CourseTaskBlockTask.fromJson(Map<String, dynamic> json) {
    return CourseTaskBlockTask(
      id: json["id"] as int,
      title: json["title"],
      isDone: false,
      taskType: taskTypeFromString(json["taskType"]),
      maxPoints: json["maxPoints"] as int,
      variants:
          (json["variants"] as List?)
              ?.map(
                (variantJson) =>
                    CourseTaskBlockTaskVariant.fromJson(variantJson),
              )
              .toList(),
    );
  }
}

class CourseTaskBlockTaskVariant {
  final int id;
  final String description;

  CourseTaskBlockTaskVariant({required this.id, required this.description});

  factory CourseTaskBlockTaskVariant.fromJson(Map<String, dynamic> json) {
    return CourseTaskBlockTaskVariant(
      id: json["id"] as int,
      description: json["description"] as String,
    );
  }
}
