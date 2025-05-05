import 'package:flutter/cupertino.dart';

import 'blocks/summaryBlock/summary_block.dart';
import 'blocks/tasksBlock/tasks_block.dart';
import 'blocks/videoBlock/video_block.dart';

class BlockView extends StatelessWidget {
  final int? blockId;
  final dynamic hierarchy;

  const BlockView({
    super.key,
    required this.blockId,
    required this.hierarchy,
  });

  @override
  Widget build(BuildContext context) {
    dynamic block;
    for (var module in hierarchy['modules']){
      for (var b in module['blocks']){
        if (b['id'] == blockId){
          block = b;
          break;
        }
      }
      if (block != null) break;
    }

    if (block == null){
      return const Center(child: Text('Блок не найден'),);
    }
    return SingleChildScrollView(
      padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 20),
      child: ConstrainedBox(
        constraints: const BoxConstraints(minWidth: 480),
        child: Builder(
          builder: (context) {
            switch (block['blockType']) {
              case 1:
                return SummaryBlock(block: block);
              case 2:
                return VideoBlock(block: block);
              case 3:
                return TasksBlock(block: block);
              default:
                return const Text('Блок не найден');
            }
          }
        )
      )
    );
  }

}
