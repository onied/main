import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:go_router/go_router.dart';

import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc_event.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc_state.dart';
import 'package:onied_mobile/pages/authorization/authorization_page.dart';
import 'package:onied_mobile/pages/authorization/registration_page.dart';
import 'package:onied_mobile/pages/home/home_page.dart';
import 'package:onied_mobile/providers/authorization_provider.dart';
import 'package:onied_mobile/providers/user_provider.dart';
import 'package:onied_mobile/providers/vk_auth_provider.dart';
import 'package:onied_mobile/repositories/user_repository.dart';

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    // Инициализация зависимостей
    final flutterSecureStorage = FlutterSecureStorage();
    final vkAuthProvider = VKAuthProvider();
    final authorizationProvider = AuthorizationProvider(
      flutterSecureStorage: flutterSecureStorage,
    );
    final userProvider = UserProvider();
    final userRepository = UserRepository(
      authorizationProvider: authorizationProvider,
      userProvider: userProvider,
    );

    return MultiBlocProvider(
      providers: [
        BlocProvider(
          create:
              (context) => AuthorizationBloc(
                repository: userRepository,
                vkAuthProvider: vkAuthProvider,
              )..add(AppStarted()), // Инициализация при старте
        ),
        // Здесь можно добавить другие Bloc-провайдеры
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

final _router = GoRouter(
  initialLocation: '/login',
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => const HomePage(),
      redirect: (context, state) {
        // Проверяем состояние авторизации
        final authState = context.read<AuthorizationBloc>().state;
        if (authState is! LoadedState) {
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
        if (authState is LoadedState) {
          return '/';
        }
        return null;
      },
    ),
    GoRoute(
      path: '/register',
      builder: (context, state) => const RegistrationPage(),
    ),
    // ... остальные маршруты без изменений ...
  ],
);
