import 'package:onied_mobile/models/enums/block_type.dart';

class CourseHierarchyModel {
  final int id;
  final String title;
  final List<CourseHierarchyModule> modules;

  CourseHierarchyModel({
    required this.id,
    required this.title,
    required this.modules,
  });

  factory CourseHierarchyModel.fromJson(Map<String, dynamic> json) {
    return CourseHierarchyModel(
      id: json['id'] as int,
      title: json['title'],
      modules:
          (json['modules'] as List)
              .map((moduleJson) => CourseHierarchyModule.fromJson(moduleJson))
              .toList(),
    );
  }
}

class CourseHierarchyModule {
  final int id;
  final String title;
  final int index;
  final List<CourseHierarchyBlock> blocks;

  CourseHierarchyModule({
    required this.id,
    required this.title,
    required this.index,
    required this.blocks,
  });

  factory CourseHierarchyModule.fromJson(Map<String, dynamic> json) {
    return CourseHierarchyModule(
      id: json['id'] as int,
      title: json['title'] as String,
      index: json['index'] as int,
      blocks:
          (json['blocks'] as List)
              .map((blockJson) => CourseHierarchyBlock.fromJson(blockJson))
              .toList(),
    );
  }
}

class CourseHierarchyBlock {
  final int id;
  final String title;
  final int index;
  final BlockType blockType;

  CourseHierarchyBlock({
    required this.id,
    required this.title,
    required this.index,
    required this.blockType,
  });

  factory CourseHierarchyBlock.fromJson(Map<String, dynamic> json) {
    return CourseHierarchyBlock(
      id: json['id'] as int,
      title: json['title'] as String,
      index: json['index'] as int,
      blockType: blockTypeFromString(json["blockType"] as String),
    );
  }
}
