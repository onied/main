import 'package:flutter/cupertino.dart';
import 'package:onied_mobile/models/course_block_model.dart';
import 'package:onied_mobile/models/enums/block_type.dart';

import 'blocks/summaryBlock/summary_block.dart';
import 'blocks/tasksBlock/tasks_block.dart';
import 'blocks/videoBlock/video_block.dart';

class BlockView extends StatelessWidget {
  final CourseBlockModel block;

  const BlockView({super.key, required this.block});

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 20),
      child: ConstrainedBox(
        constraints: const BoxConstraints(minWidth: 480),
        child: Builder(
          builder: (context) {
            switch (block.blockType) {
              case BlockType.summaryBlock:
                return SummaryBlock(block: block as CourseSummaryBlockModel);
              case BlockType.videoBlock:
                return VideoBlock(block: block as CourseVideoBlockModel);
              case BlockType.tasksBlock:
                return TasksBlock(block: block as CourseTaskBlockModel);
              default:
                return const Text('Блок не найден');
            }
          },
        ),
      ),
    );
  }
}
