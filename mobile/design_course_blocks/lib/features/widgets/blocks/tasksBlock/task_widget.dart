import 'package:design_course_blocks/features/widgets/blocks/tasksBlock/tasks/input_answer_task.dart';
import 'package:design_course_blocks/features/widgets/blocks/tasksBlock/tasks/manual_review_task.dart';
import 'package:design_course_blocks/features/widgets/blocks/tasksBlock/tasks/multiple_answer_task.dart';
import 'package:design_course_blocks/features/widgets/blocks/tasksBlock/tasks/single_answer_task.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class TaskWidget extends StatelessWidget {
  final dynamic task;
  final Function(Map<String, dynamic>) onChange;
  final int index;

  const TaskWidget({
    super.key,
    required this.task,
    required this.onChange,
    required this.index,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(vertical: 10),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            "${index + 1}. ${task['title']}",
            style: Theme.of(context).textTheme.titleLarge?.copyWith(color: Colors.black),
          ),
          const SizedBox(height: 10,),
          Container(
            padding: EdgeInsets.symmetric(horizontal: 12, vertical: 6),
            decoration: BoxDecoration(
              color: Colors.black,
              borderRadius: BorderRadius.circular(8),
            ),
            child: Text(
                'Непроверено',
                style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                    color: Colors.white
                )
            ),
          ),
          const SizedBox(height: 10,),
          Container(
            padding: const EdgeInsets.symmetric(horizontal: 16),
            child: Builder(
              builder: (context) {
                switch (task['taskType']) {
                  case 0:
                    return SingleAnswerTask(
                      task: task,
                      onChange: onChange,
                    );
                  case 1:
                    return MultipleAnswersTask(
                      task: task,
                      onChange: onChange,
                    );
                  case 2:
                    return InputAnswerTask(
                      task: task,
                      onChange: onChange,
                    );
                  case 3:
                    return ManualReviewTask(
                      task: task,
                      onChange: onChange,
                    );
                  default:
                    return const Text('Задача не найдена');
                }
              },
            )
          ),
        ],
      ),
    );
  }

}
