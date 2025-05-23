import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

class RedirectToRegistrationForm extends StatelessWidget {
  const RedirectToRegistrationForm({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Text("Нет аккаунта?"),
        Padding(
          padding: EdgeInsets.only(top: 20.0),
          child: Row(
            mainAxisSize: MainAxisSize.max,
            children: [
              Expanded(
                child: FilledButton(
                  onPressed: () {
                    context.go("/register");
                  },
                  child: Text("зарегистрироваться"),
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
}
