import 'package:onied_mobile/requests/forms/login_form_data.dart';
import 'package:onied_mobile/requests/forms/registration_form_data.dart';

abstract class AuthorizationBlocEvent {}

class AppStarted extends AuthorizationBlocEvent {}

class LoginWithVk extends AuthorizationBlocEvent {}

class Register extends AuthorizationBlocEvent {
  final RegistrationFormData formData;

  Register({required this.formData});
}

class Login extends AuthorizationBlocEvent {
  final LoginFormData formData;

  Login({required this.formData});
}
