import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/app/config.dart';
import 'package:onied_mobile/form_data/login_vk_form_data.dart';
import 'package:onied_mobile/providers/vk_auth_service.dart';
import 'package:onied_mobile/repositories/user_repository.dart';

import 'authorization_bloc_event.dart';
import 'authorization_bloc_state.dart';

class AuthorizationBloc
    extends Bloc<AuthorizationBlocEvent, AuthorizationBlocState> {
  final UserRepository repository;
  final VKAuthService vkAuthService;

  AuthorizationBloc({required this.repository, required this.vkAuthService})
    : super(LoadingState()) {
    on<Login>(_onLogin);
    on<LoginWithVk>(_onLoginVk);
    on<Register>(_onRegister);
  }

  Future<void> _onLogin(
    Login event,
    Emitter<AuthorizationBlocState> emit,
  ) async {
    repository.login(event.formData);
  }

  Future<void> _onLoginVk(
    LoginWithVk event,
    Emitter<AuthorizationBlocState> emit,
  ) async {
    final code = await vkAuthService.authViaVK();
    if (code == null) return;
    repository.loginVk(
      LoginVkFormData(code: code, redirectUri: Config.redirectUri),
    );
  }

  Future<void> _onRegister(
    Register event,
    Emitter<AuthorizationBlocState> emit,
  ) async {
    repository.register(event.formData);
  }
}
