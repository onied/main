import 'package:onied_mobile/app/app_theme.dart';
import 'package:flutter/material.dart';
import 'package:onied_mobile/views/authorization/authorization_page.dart';
import 'package:onied_mobile/views/authorization/registration_page.dart';
import 'package:onied_mobile/views/course/course_page.dart';
import 'package:onied_mobile/views/profile_info/profile_info.dart';
import 'package:onied_mobile/views/purchase/purchase_page.dart';

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: "Onied",
      theme: AppTheme.main,
      initialRoute: "/login",
      routes: {
        "/profile": (context) => const ProfileInfoPage(),
        "/course/{courseId}": (context) => const CoursePage(),
        "/purchase": (context) => const PurchasePage(),
        "/login": (context) => const AuthorizationPage(),
        "/register": (context) => const RegistrationPage(),
      },
      debugShowCheckedModeBanner: false,
    );
  }
}
