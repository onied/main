import 'package:design_course_blocks/app/app_theme.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

import '../features/pages/course_page.dart';

class CourseBlocks extends StatelessWidget {
  const CourseBlocks({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: "Course Blocks",
      theme: AppTheme.ThemeDataCourseBlocks,
      home: const CoursePage(),
      debugShowCheckedModeBanner: false,
    );
  }

}
