import 'package:flutter/material.dart';

import 'package:mobile_authorization/widgets/form_divider.dart';
import 'package:mobile_authorization/widgets/forms/redirect_to_login_form.dart';
import 'package:mobile_authorization/widgets/forms/vk_login_form.dart';

import '../widgets/forms/registration_form.dart';

class RegistrationPage extends StatelessWidget {
  const RegistrationPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body:
        SingleChildScrollView(
          child: Padding(
            padding: EdgeInsets.only(
                left: 30.0,
                top: 10.0,
                right: 30.0,
                bottom: 10.0
            ),
            child: Center(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.center,
                children: [
                  Padding(
                    padding: EdgeInsets.only(bottom: 20),
                    child: Text(
                      "OniEd",
                      style: TextStyle(
                          fontSize: 48
                      ),
                    ),
                  ),
                  VkLoginForm(),
                  Padding(
                    padding: EdgeInsets.only(
                        top: 20.0,
                        bottom: 20.0
                    ),
                    child: FormDivider(),
                  ),
                  RegistrationForm(),
                  Padding(
                    padding: EdgeInsets.only(
                      top: 30.0,
                    ),
                    child: RedirectToLoginForm(),
                  )
                ]
              )
            )
          )
        )
    );
  }
}
