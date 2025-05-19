import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc_state.dart';

import 'components/form_divider.dart';
import 'components/forms/login_form.dart';
import 'components/forms/redirect_to_registration_form.dart';
import 'components/forms/vk_login_form.dart';

class AuthorizationPage extends StatelessWidget {
  const AuthorizationPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(),
      body: BlocBuilder<AuthorizationBloc, AuthorizationBlocState>(
        builder: (context, state) {
          return switch (state) {
            LoadingState() => const Center(child: CircularProgressIndicator()),
            AuthorizedState() => const Center(
              child: CircularProgressIndicator(),
            ),
            _ => SingleChildScrollView(
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
          };
        },
      ),
    );
  }
}
