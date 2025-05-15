import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;
import 'package:onied_mobile/app/config.dart';
import 'package:onied_mobile/form_data/login_form_data.dart';
import 'package:onied_mobile/form_data/login_vk_form_data.dart';
import 'package:onied_mobile/form_data/registration_form_data.dart';
import 'package:onied_mobile/models/enums/gender.dart';
import 'package:onied_mobile/providers/authorization_api.dart';
import 'package:onied_mobile/models/user_model.dart';

// TODO: ALL API CALLS SHOULD BE IN PROVIDERS!!!!!

class UserRepository {
  Future<AuthorizationData?> login(LoginFormData formData) async {
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

  Future<AuthorizationData?> loginVk(LoginVkFormData formData) async {
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

  Future<AuthorizationData?> register(RegistrationFormData formData) async {
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

  Future<UserModel?> getProfile() async {
    return UserModel(
      firstName: "Admin",
      lastName: "Admin",
      gender: Gender.other,
      email: "admin@admin.admin",
    );
  }
}
