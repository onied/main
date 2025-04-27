import 'package:design_course_blocks/features/widgets/blocks/tasksBlock/task_widget.dart';
import 'package:flutter/material.dart';

import '../../../../shared/button/button.dart';

class TasksBlock extends StatefulWidget {
  final dynamic block;

  const TasksBlock({
    super.key,
    required this.block,
  });

  @override
  State<TasksBlock> createState() => _TasksBlockState();
}

class _TasksBlockState extends State<TasksBlock> {
  final List<Map<String, dynamic>> taskInputs = [];

  @override
  void initState(){
    super.initState();
    for (var task in widget.block['tasks'] ?? []){
      taskInputs.add({
        'taskId': task['id'],
        'isDone': false,
        'taskType': task['taskType'],
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          widget.block['title'],
          style: Theme.of(context).textTheme.headlineMedium,
        ),
        const SizedBox(height: 20,),
        Form(
          child: Column(
            children: [
              ...List.generate(
                widget.block['tasks']?.length ?? 0,
                (index) {
                  final task = widget.block['tasks'][index];
                  return TaskWidget(
                    index: index,
                    task: task,
                    onChange: (input) {
                      setState(() {
                        taskInputs[index] = input;
                      });
                    }
                  );
                }
              ),
            ],
          ),
        ),
        const SizedBox(height: 20,),
        if ((widget.block['tasks']?.length ?? 0) == 0)
          Center(
            child: Text(
              'Задач нет',
              style: Theme.of(context).textTheme.titleLarge,
            ),
          )
        else
          Button(
            onPressed: () {},
            textButton: "Отправить на проверку",
          )
      ],
    );
  }

}
