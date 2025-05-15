import 'package:flutter/material.dart';
import 'package:onied_mobile/models/course_block_model.dart';

class InputAnswerTask extends StatefulWidget {
  final CourseTaskBlockTask task;
  final Function(Map<String, dynamic>) onChange;

  const InputAnswerTask({
    super.key,
    required this.task,
    required this.onChange,
  });

  @override
  State<InputAnswerTask> createState() => _InputAnswerTaskState();
}

class _InputAnswerTaskState extends State<InputAnswerTask> {
  final TextEditingController _controller = TextEditingController();

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return TextField(
      controller: _controller,
      decoration: const InputDecoration(
        border: OutlineInputBorder(),
        hintText: 'Введите свой ответ',
      ),
      onChanged: (value) {
        widget.onChange({
          'taskId': widget.task.id,
          'taskType': widget.task.taskType,
          'isDone': widget.task.isDone,
          'answer': value,
        });
      },
      minLines: 1,
      maxLines: 5,
    );
  }
}
