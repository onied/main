import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/app/config.dart';
import 'package:onied_mobile/models/auth/credentials.dart';
import 'package:onied_mobile/requests/forms/login_vk_form_data.dart';
import 'package:onied_mobile/providers/vk_auth_provider.dart';
import 'package:onied_mobile/repositories/user_repository.dart';
import 'package:onied_mobile/models/enums/auth_status.dart';
import 'package:onied_mobile/requests/refresh_request.dart';

import 'authorization_bloc_event.dart';
import 'authorization_bloc_state.dart';

class AuthorizationBloc
    extends Bloc<AuthorizationBlocEvent, AuthorizationBlocState> {
  final UserRepository repository;
  final VKAuthProvider vkAuthProvider;
  final GoRouter router;

  AuthorizationBloc({
    required this.repository,
    required this.vkAuthProvider,
    required this.router,
  }) : super(LoadingState()) {
    on<Login>(_onLogin);
    on<LoginWithVk>(_onLoginVk);
    on<Register>(_onRegister);
    on<AppStarted>(_onAppStarted);
  }

  Future<void> _onLogin(
    Login event,
    Emitter<AuthorizationBlocState> emit,
  ) async {
    emit(LoadingState());
    try {
      final authStatus = await repository.login(event.formData);

      if (authStatus == AuthStatus.ok) {
        emit(AuthorizedState());
        router.go("/");
      } else if (authStatus == AuthStatus.requiresTwoFactor) {
        emit(RequiresTwoFactorState());
      } else {
        emit(
          ErrorState(errorMessage: _mapAuthStatusToErrorMessage(authStatus)),
        );
      }
    } catch (e) {
      emit(ErrorState(errorMessage: 'Login failed: ${e.toString()}'));
    }
  }

  Future<void> _onLoginVk(
    LoginWithVk event,
    Emitter<AuthorizationBlocState> emit,
  ) async {
    emit(LoadingState());
    try {
      final code = await vkAuthProvider.authViaVK();
      if (code == null) {
        emit(ErrorState(errorMessage: 'VK authorization canceled'));
        return;
      }

      await repository.loginVk(
        LoginVkFormData(code: code, redirectUri: Config.redirectUri),
      );
      emit(AuthorizedState());
      router.go("/");
    } catch (e) {
      emit(ErrorState(errorMessage: 'VK login failed: ${e.toString()}'));
    }
  }

  Future<void> _onRegister(
    Register event,
    Emitter<AuthorizationBlocState> emit,
  ) async {
    emit(LoadingState());
    try {
      await repository.register(event.formData);
      emit(AuthorizedState());
      router.go("/");
    } catch (e) {
      emit(ErrorState(errorMessage: 'Registration failed: ${e.toString()}'));
    }
  }

  Future<void> _onAppStarted(
    AppStarted event,
    Emitter<AuthorizationBlocState> emit,
  ) async {
    try {
      emit(LoadingState());
      final isAuthenticated =
          await repository.authorizationProvider.isAuthenticated();
      if (isAuthenticated) {
        emit(AuthorizedState());
        router.go("/");
      } else {
        Credentials? authData =
            await repository.authorizationProvider.getCredentials();
        if (authData == null) {
          emit(NotAuthorizedState());
          return;
        }

        authData = await repository.userProvider.refresh(
          RefreshRequest(refreshToken: authData.refreshToken),
        );
        if (authData == null) {
          emit(NotAuthorizedState());
        } else {
          await repository.authorizationProvider.setupCredentials(authData);
          emit(AuthorizedState());
          router.go("/");
        }
      }
    } catch (e) {
      emit(ErrorState(errorMessage: 'Initialization error: ${e.toString()}'));
    }
  }

  String _mapAuthStatusToErrorMessage(AuthStatus status) {
    switch (status) {
      case AuthStatus.requiresTwoFactor:
        return 'Two-factor authentication required';
      case AuthStatus.failure:
      default:
        return 'Authentication failed';
    }
  }
}
