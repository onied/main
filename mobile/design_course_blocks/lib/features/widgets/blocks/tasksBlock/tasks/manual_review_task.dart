import 'package:flutter/material.dart';

class ManualReviewTask extends StatefulWidget {
  final dynamic task;
  final Function(Map<String, dynamic>) onChange;

  const ManualReviewTask({
    super.key,
    required this.task,
    required this.onChange,
  });

  @override
  State<ManualReviewTask> createState() => _ManualReviewTaskState();
}

class _ManualReviewTaskState extends State<ManualReviewTask> {
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
          'taskId': widget.task['id'],
          'taskType': widget.task['taskType'],
          'isDone': widget.task['isDone'],
          'text': value,
        });
      },
      minLines: 5,
      maxLines: 10,
      keyboardType: TextInputType.multiline,
    );
  }
}
