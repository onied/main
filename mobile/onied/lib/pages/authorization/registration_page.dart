import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc.dart';
import 'package:onied_mobile/providers/vk_auth_service.dart';
import 'package:onied_mobile/repositories/user_repository.dart';

import 'components/form_divider.dart';
import 'components/forms/redirect_to_login_form.dart';
import 'components/forms/vk_login_form.dart';
import 'components/forms/registration_form.dart';

class RegistrationPage extends StatelessWidget {
  const RegistrationPage({super.key});

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
                children: [
                  Padding(
                    padding: EdgeInsets.only(bottom: 20),
                    child: Text("OniEd", style: TextStyle(fontSize: 48)),
                  ),
                  VkLoginForm(),
                  Padding(
                    padding: EdgeInsets.only(top: 20.0, bottom: 20.0),
                    child: FormDivider(),
                  ),
                  RegistrationForm(),
                  Padding(
                    padding: EdgeInsets.only(top: 30.0),
                    child: RedirectToLoginForm(),
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
