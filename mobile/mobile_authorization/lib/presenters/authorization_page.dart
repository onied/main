import 'package:flutter/material.dart';
import 'package:mobile_authorization/widgets/form_divider.dart';
import 'package:mobile_authorization/widgets/forms/login_form.dart';
import 'package:mobile_authorization/widgets/forms/redirect_to_registration_form.dart';
import 'package:mobile_authorization/widgets/forms/vk_login_form.dart';

class AuthorizationPage extends StatelessWidget {
  const AuthorizationPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body:
        SingleChildScrollView(
          child: Padding(
            padding: EdgeInsets.only(
                left: 30.0,
                top: 150.0,
                right: 30.0,
                bottom: 30.0
            ),
            child: Center(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                spacing: 3,
                children: [
                  Padding(
                    padding: EdgeInsets.only(bottom: 30),
                    child: Text(
                      "OniEd",
                      style: TextStyle(
                          fontSize: 48
                      ),
                    ),
                  ),
                  LoginForm(),
                  Padding(
                    padding: EdgeInsets.only(
                        top: 30.0,
                        bottom: 30.0
                    ),
                    child: FormDivider(),
                  ),
                  VkLoginForm(),
                  Padding(
                    padding: EdgeInsets.only(
                      top: 30.0,
                    ),
                    child: RedirectToRegistrationForm(),
                  )
                ]
              )
            )
          )
        )
    );
  }
}
