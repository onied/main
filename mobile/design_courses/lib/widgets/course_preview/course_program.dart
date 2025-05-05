import 'package:flutter/material.dart';

class CourseProgram extends StatelessWidget {
  final List<String>? modules;

  const CourseProgram({super.key, required this.modules});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const Padding(
          padding: EdgeInsets.symmetric(vertical: 8.0),
          child: Text(
            'Программа курса',
            style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
          ),
        ),
        ...(modules?.map(
              (moduleTitle) => Padding(
                padding: const EdgeInsets.only(left: 16.0, bottom: 8.0),
                child: Text(
                  '• $moduleTitle',
                  style: const TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                    height: 2,
                  ),
                ),
              ),
            ) ??
            []),
      ],
    );
  }
}
