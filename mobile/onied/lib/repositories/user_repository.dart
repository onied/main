import 'package:onied_mobile/models/auth/credentials.dart';
import 'package:onied_mobile/models/enums/auth_status.dart';
import 'package:onied_mobile/requests/profile_changed_request.dart';
import 'package:onied_mobile/requests/refresh_request.dart';
import 'package:webview_flutter/webview_flutter.dart';

import 'package:onied_mobile/providers/user_provider.dart';
import 'package:onied_mobile/providers/authorization_provider.dart';
import 'package:onied_mobile/requests/forms/login_form_data.dart';
import 'package:onied_mobile/requests/forms/login_vk_form_data.dart';
import 'package:onied_mobile/requests/forms/registration_form_data.dart';
import 'package:onied_mobile/models/user_model.dart';

class UserRepository {
  final AuthorizationProvider authorizationProvider;
  final UserProvider userProvider;

  const UserRepository({
    required this.authorizationProvider,
    required this.userProvider,
  });

  Future<AuthStatus> login(LoginFormData formData) async {
    final authData = await userProvider.login(formData);

    if (authData.authStatus == AuthStatus.ok) {
      authorizationProvider.setupCredentials(authData.credentials!);
      return AuthStatus.ok;
    } else {
      return authData.authStatus;
    }
  }

  Future<void> loginVk(LoginVkFormData formData) async {
    final authData = await userProvider.loginVk(formData);

    if (authData == null) {
      throw HttpResponseError();
    }

    authorizationProvider.setupCredentials(authData);
  }

  Future<void> register(RegistrationFormData formData) async {
    final authData = await userProvider.register(formData);

    if (authData == null) {
      throw HttpResponseError();
    }

    authorizationProvider.setupCredentials(authData);
  }

  Future<UserModel?> getProfile() async {
    final authData = await _tryGetCredentials();
    if (authData == null) {
      return null;
    }

    return userProvider.getProfile(authData);
  }

  Future<UserModel?> updateProfile(ProfileChangedRequest request) async {
    final authData = await _tryGetCredentials();
    if (authData == null) {
      return null;
    }

    return userProvider.updateProfile(request, authData);
  }

  Future<Credentials?> _tryGetCredentials() async {
    late Credentials? authData;
    if (!await authorizationProvider.isAuthenticated()) {
      final prevData = await authorizationProvider.getCredentials();
      if (prevData != null) {
        authData = await userProvider.refresh(
          RefreshRequest(refreshToken: prevData.refreshToken),
        );
      }
    } else {
      authData = await authorizationProvider.getCredentials();
    }

    return authData;
  }
}
