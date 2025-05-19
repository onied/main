import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';

import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/app/injection.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc_event.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc_state.dart';
import 'package:onied_mobile/pages/authorization/authorization_page.dart';
import 'package:onied_mobile/pages/authorization/registration_page.dart';
import 'package:onied_mobile/pages/catalog/catalog_page.dart';
import 'package:onied_mobile/pages/course/course_page.dart';
import 'package:onied_mobile/pages/course_preview/course_preview_page.dart';
import 'package:onied_mobile/pages/home/home_page.dart';
import 'package:onied_mobile/pages/profile_info/profile_info.dart';
import 'package:onied_mobile/pages/purchase/purchase_page.dart';
import 'package:onied_mobile/providers/vk_auth_provider.dart';
import 'package:onied_mobile/repositories/user_repository.dart';

final _router = GoRouter(
  initialLocation: '/login',
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => const HomePage(),
      redirect: (context, state) {
        // Проверяем состояние авторизации
        final authState = context.read<AuthorizationBloc>().state;
        if (authState is! AuthorizedState) {
          return '/login';
        }
        return null;
      },
    ),
    GoRoute(
      path: '/login',
      builder: (context, state) => const AuthorizationPage(),
      redirect: (context, state) {
        final authState = context.read<AuthorizationBloc>().state;
        if (authState is AuthorizedState) {
          return '/';
        }
        return null;
      },
    ),
    GoRoute(
      path: '/register',
      builder: (context, state) => const RegistrationPage(),
    ),
    GoRoute(
      path: '/profile',
      builder: (context, state) => const ProfileInfoPage(),
    ),
    GoRoute(path: '/purchase', builder: (context, state) => PurchasePage()),
    GoRoute(
      path: '/course/:id',
      builder:
          (context, state) =>
              CoursePreviewPage(courseId: state.pathParameters["id"]!),
    ),
    GoRoute(
      path: '/course/:id/learn',
      builder:
          (context, state) =>
              CoursePage(id: int.parse(state.pathParameters["id"]!)),
    ),
    GoRoute(path: '/search', builder: (context, state) => const CatalogPage()),
  ],
);

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider(
          create:
              (context) => AuthorizationBloc(
                repository: getIt<UserRepository>(),
                vkAuthProvider: getIt<VKAuthProvider>(),
                router: _router,
              )..add(AppStarted()),
        ),
      ],
      child: MaterialApp.router(
        title: "OniEd",
        theme: AppTheme.main,
        routerConfig: _router,
        debugShowCheckedModeBanner: false,
      ),
    );
  }
}
