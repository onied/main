import 'package:onied_mobile/form_data/login_form_data.dart';
import 'package:onied_mobile/form_data/registration_form_data.dart';

abstract class AuthorizationBlocEvent {}

class LoginWithVk extends AuthorizationBlocEvent {}

class Register extends AuthorizationBlocEvent {
  final RegistrationFormData formData;

  Register({required this.formData});
}

class Login extends AuthorizationBlocEvent {
  final LoginFormData formData;

  Login({required this.formData});
}
