import 'package:flutter/material.dart';
import 'package:onied_mobile/models/course_block_model.dart';

class MultipleAnswersTask extends StatefulWidget {
  final CourseTaskBlockTask task;
  final Function(Map<String, dynamic>) onChange;

  const MultipleAnswersTask({
    super.key,
    required this.task,
    required this.onChange,
  });

  @override
  State<MultipleAnswersTask> createState() => _MultipleAnswersTaskState();
}

class _MultipleAnswersTaskState extends State<MultipleAnswersTask> {
  final List<int> selectedValues = [];

  @override
  Widget build(BuildContext context) {
    return Column(
      children:
          (widget.task.variants ?? []).map<Widget>((variant) {
            return Container(
              padding: const EdgeInsets.symmetric(vertical: 4),
              child: Row(
                children: [
                  Checkbox(
                    value: selectedValues.contains(variant.id),
                    onChanged: (value) {
                      setState(() {
                        if (value == true) {
                          selectedValues.add(variant.id);
                        } else {
                          selectedValues.remove(variant.id);
                        }
                      });
                      widget.onChange({
                        'taskId': widget.task.id,
                        'taskType': widget.task.taskType,
                        'isDone': widget.task.isDone,
                        'variantsIds': selectedValues,
                      });
                    },
                  ),
                  const SizedBox(width: 4),
                  Expanded(
                    child: Text(
                      variant.description,
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
