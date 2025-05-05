import 'package:go_router/go_router.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:flutter/material.dart';
import 'package:onied_mobile/views/authorization/authorization_page.dart';
import 'package:onied_mobile/views/authorization/registration_page.dart';
import 'package:onied_mobile/views/catalog/search_results_page.dart';
import 'package:onied_mobile/views/course/course_page.dart';
import 'package:onied_mobile/views/course_preview/course_preview_page.dart';
import 'package:onied_mobile/views/home/home_page.dart';
import 'package:onied_mobile/views/profile_info/profile_info.dart';
import 'package:onied_mobile/views/purchase/purchase_page.dart';

final _router = GoRouter(
  initialLocation: '/login',
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => const HomePage(),
    ),
    GoRoute(
      path: '/login',
      builder: (context, state) => const AuthorizationPage(),
    ),
    GoRoute(
      path: '/register',
      builder: (context, state) => const RegistrationPage(),
    ),
    GoRoute(
      path: '/profile',
      builder: (context, state) => const ProfileInfoPage(),
    ),
    GoRoute(
      path: '/purchase',
      builder: (context, state) => const PurchasePage(),
    ),
    GoRoute(
      path: '/course/:id',
      builder: (context, state) => PreviewPage(courseId: state.pathParameters["id"]!),
    ),
    GoRoute(
      path: '/course/:id/learn',
      builder: (context, state) => CoursePage(id: state.pathParameters["id"]!),
    ),
    GoRoute(
      path: '/search',
      builder: (context, state) => const SearchResultsPage(),
    ),
  ],
);

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      title: "OniEd",
      theme: AppTheme.main,
      routerConfig: _router,
      debugShowCheckedModeBanner: false,
    );
  }
}
