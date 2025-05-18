import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc.dart';
import 'package:onied_mobile/providers/vk_auth_service.dart';
import 'package:onied_mobile/repositories/user_repository.dart';
import 'components/form_divider.dart';
import 'components/forms/login_form.dart';
import 'components/forms/redirect_to_registration_form.dart';
import 'components/forms/vk_login_form.dart';

class AuthorizationPage extends StatelessWidget {
  const AuthorizationPage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) => AuthorizationBloc(
            repository: UserRepository(),
            vkAuthService: VKAuthService(),
          ),
      child: Scaffold(
        appBar: AppBar(),
        body: SingleChildScrollView(
          child: Padding(
            padding: EdgeInsets.all(30.0),
            child: Center(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                spacing: 3,
                children: [
                  Padding(
                    padding: EdgeInsets.only(bottom: 30),
                    child: Text("OniEd", style: TextStyle(fontSize: 48)),
                  ),
                  LoginForm(),
                  Padding(
                    padding: EdgeInsets.only(top: 30.0, bottom: 30.0),
                    child: FormDivider(),
                  ),
                  VkLoginForm(),
                  Padding(
                    padding: EdgeInsets.only(top: 30.0),
                    child: RedirectToRegistrationForm(),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
