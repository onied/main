import 'package:flutter/material.dart';

class RedirectToLoginForm extends StatelessWidget {
  const RedirectToLoginForm({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Text("Уже есть аккаунт?"),
        Padding(
          padding: EdgeInsets.only(
              top: 20.0
          ),
          child: Row(
            mainAxisSize: MainAxisSize.max,
            children: [
              Expanded(
                  child: FilledButton(
                      onPressed: () {
                        Navigator.pushReplacementNamed(context, "/login");
                      },
                      child: Text("войти")
                  )
              )
            ],
          ),
        ),

      ],
    );
  }
}
