import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;
import 'package:onied_mobile/app/config.dart';
import 'package:onied_mobile/models/authorization_data.dart';
import 'package:onied_mobile/models/login_form_data.dart';
import 'package:onied_mobile/models/login_vk_form_data.dart';
import 'package:onied_mobile/models/registration_form_data.dart';

class AuthorizationApi {
  static Future<AuthorizationData?> login(LoginFormData formData) async {
    final response = await http.post(
      Uri.parse("${Config.backendUrl}/login"),
      body: jsonEncode(formData),
    );

    if (response.statusCode != HttpStatus.ok) {
      return null;
    }

    return AuthorizationData.fromJson(
      jsonDecode(response.body) as Map<String, dynamic>,
    );
  }

  static Future<AuthorizationData?> loginVk(LoginVkFormData formData) async {
    final response = await http.post(
      Uri.parse("${Config.backendUrl}/signinVk"),
      body: jsonEncode(formData),
    );

    if (response.statusCode != HttpStatus.ok) {
      return null;
    }

    return AuthorizationData.fromJson(
      jsonDecode(response.body) as Map<String, dynamic>,
    );
  }

  static Future<AuthorizationData?> register(
    RegistrationFormData formData,
  ) async {
    final response = await http.post(
      Uri.parse("${Config.backendUrl}/register"),
      body: jsonEncode(formData),
    );

    if (response.statusCode != HttpStatus.ok) {
      return null;
    }

    return await login(
      LoginFormData(email: formData.email, password: formData.password),
    );
  }
}
