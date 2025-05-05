import 'package:onied_mobile/app/app_theme.dart';
import 'package:flutter/material.dart';

class SingleAnswerTask extends StatefulWidget {
  final dynamic task;
  final Function(Map<String, dynamic>) onChange;

  const SingleAnswerTask({
    super.key,
    required this.task,
    required this.onChange,
  });

  @override
  State<SingleAnswerTask> createState() => _SingleAnswerTaskState();
}

class _SingleAnswerTaskState extends State<SingleAnswerTask> {
  int? selectedValue;

  @override
  Widget build(BuildContext context) {
    return Column(
      children: (widget.task['variants'] ?? []).map<Widget>((variant) {
        return Container(
          padding: const EdgeInsets.symmetric(vertical: 4),
          child: Row(
            children: [
              Radio<int>(
                activeColor: AppTheme.accent,
                value: variant['id'],
                groupValue: selectedValue,
                onChanged: (value) {
                  setState(() {
                    selectedValue = value;
                  });
                  widget.onChange({
                    'taskId': widget.task['id'],
                    'taskType': widget.task['taskType'],
                    'isDone': widget.task['isDone'],
                    'variantsIds': [variant['id']],
                  });
                },
              ),
              const SizedBox(width: 4),
              Expanded(
                child: Text(
                  variant['description'],
                  style: Theme.of(context).textTheme.bodyLarge,
                ),
              ),
            ],
          ),
        );
      }).toList(),
    );
  }
}
