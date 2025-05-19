import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;
import 'package:logging/logging.dart';

import 'package:onied_mobile/app/config.dart';
import 'package:onied_mobile/models/auth/authorization_data.dart';
import 'package:onied_mobile/models/auth/credentials.dart';
import 'package:onied_mobile/models/enums/auth_status.dart';
import 'package:onied_mobile/models/user_model.dart';
import 'package:onied_mobile/requests/forms/login_form_data.dart';
import 'package:onied_mobile/requests/forms/login_vk_form_data.dart';
import 'package:onied_mobile/requests/forms/registration_form_data.dart';
import 'package:onied_mobile/requests/profile_changed_request.dart';
import 'package:onied_mobile/requests/refresh_request.dart';

class UserProvider {
  final _logger = Logger("UserProvider");

  Future<AuthorizationData> login(LoginFormData formData) async {
    final response = await http.post(
      Uri.parse("${Config.backendUrl}/login"),
      body: jsonEncode(formData),
      headers: {'Content-Type': 'application/json'},
    );

    if (response.statusCode != HttpStatus.ok) {
      final responseBody = jsonDecode(response.body) as Map<String, dynamic>;
      if (responseBody["detail"] == "RequiresTwoFactor") {
        return AuthorizationData(authStatus: AuthStatus.requiresTwoFactor);
      } else {
        return AuthorizationData(authStatus: AuthStatus.failure);
      }
    }

    return AuthorizationData(
      authStatus: AuthStatus.ok,
      credentials: Credentials.fromJson(
        jsonDecode(response.body) as Map<String, dynamic>,
      ),
    );
  }

  Future<Credentials?> loginVk(LoginVkFormData formData) async {
    final response = await http.post(
      Uri.parse("${Config.backendUrl}/signinVk"),
      body: jsonEncode(formData),
      headers: {'Content-Type': 'application/json'},
    );

    if (response.statusCode != HttpStatus.ok) {
      return null;
    }

    return Credentials.fromJson(
      jsonDecode(response.body) as Map<String, dynamic>,
    );
  }

  Future<Credentials?> register(RegistrationFormData formData) async {
    final response = await http.post(
      Uri.parse("${Config.backendUrl}/register"),
      body: jsonEncode(formData),
      headers: {'Content-Type': 'application/json'},
    );

    if (response.statusCode != HttpStatus.ok) {
      return null;
    }

    final loginResponse = await login(
      LoginFormData(email: formData.email, password: formData.password),
    );
    return loginResponse.credentials;
  }

  Future<Credentials?> refresh(RefreshRequest request) async {
    final response = await http.post(
      Uri.parse("${Config.backendUrl}/refresh"),
      body: jsonEncode(request),
      headers: {'Content-Type': 'application/json'},
    );

    if (response.statusCode != HttpStatus.ok) {
      return null;
    }

    return Credentials.fromJson(
      jsonDecode(response.body) as Map<String, dynamic>,
    );
  }

  Future<UserModel?> getProfile(Credentials credentials) async {
    final response = await http.get(
      Uri.parse("${Config.backendUrl}/profile"),
      headers: {'Authorization': 'Bearer ${credentials.accessToken}'},
    );

    if (response.statusCode != HttpStatus.ok) {
      return null;
    }

    return UserModel.fromJson(
      jsonDecode(response.body) as Map<String, dynamic>,
    );
  }

  Future<UserModel?> updateProfile(
    ProfileChangedRequest request,
    Credentials credentials,
  ) async {
    final response = await http.put(
      Uri.parse("${Config.backendUrl}/profile"),
      body: jsonEncode(request),
      headers: {
        'Authorization': 'Bearer ${credentials.accessToken}',
        'Content-Type': 'application/json',
      },
    );

    if (response.statusCode != HttpStatus.ok) {
      return null;
    }

    return getProfile(credentials);
  }
}
