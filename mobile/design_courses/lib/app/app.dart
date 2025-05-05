import 'package:design_courses/data/repository_mock.dart';
import 'package:design_courses/pages/course_preview_page.dart';
import 'package:design_courses/app/app_theme.dart';
import 'package:design_courses/pages/home_page.dart';
import 'package:flutter/material.dart';

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: "Design courses",
      theme: AppTheme.appThemeData,
      home: HomePage(),
      debugShowCheckedModeBanner: false,
    );
  }
}
