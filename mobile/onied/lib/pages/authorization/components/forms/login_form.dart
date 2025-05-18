import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:logging/logging.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc_event.dart';
import 'package:onied_mobile/requests/forms/login_form_data.dart';
import 'package:onied_mobile/utils/email_validator.dart';

class LoginForm extends StatefulWidget {
  const LoginForm({super.key});

  @override
  State<StatefulWidget> createState() => _LoginFormState();
}

class _LoginFormState extends State<LoginForm> {
  final _formKey = GlobalKey<FormState>();
  final _logger = Logger("LoginFormState");

  late String _email;
  late String _password;
  late String? _code;
  bool _passwordVisible = false;
  bool _twoFARequired = false;

  void _enableCodeTextField() {
    setState(() {
      _twoFARequired = true;
    });
  }

  void _disableCodeTextField() {
    setState(() {
      _twoFARequired = false;
    });
  }

  Future<void> _trySendLoginRequest() async {
    if (!_formKey.currentState!.validate()) {
      return;
    }

    if (_twoFARequired && _code != null && _code!.isNotEmpty) {
      _disableCodeTextField(); // TODO: убрать при внедрении логики!!!
      ScaffoldMessenger.of(
        context,
      ).showSnackBar(const SnackBar(content: Text('Запрос к серверу...')));
      final formData = LoginFormData(
        email: _email,
        password: _password,
        twoFactorCode: _code,
      );
      _logger.log(Level.INFO, "Trying to log in ${jsonEncode(formData)}...");
      context.read<AuthorizationBloc>().add(Login(formData: formData));
      context.push("/");
    } else {
      _enableCodeTextField();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Form(
      key: _formKey,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          TextFormField(
            validator: (value) {
              if (value == null || value.isEmpty) {
                return "поле не может быть пустым";
              } else if (!value.isValidEmail()) {
                return "такой почты не существует";
              }
              return null;
            },
            decoration: const InputDecoration(hintText: "Эл. почта"),
            onChanged: (value) {
              setState(() {
                _email = value;
              });
            },
          ),
          Padding(
            padding: const EdgeInsets.only(top: 10.0),
            child: TextFormField(
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "поле не может быть пустым";
                }
                return null;
              },
              obscureText: !_passwordVisible,
              decoration: InputDecoration(
                hintText: "пароль",
                suffixIcon: IconButton(
                  icon: Icon(
                    _passwordVisible ? Icons.visibility : Icons.visibility_off,
                    color:
                        _passwordVisible
                            ? AppTheme.accentDark
                            : AppTheme.textSecondaryColor,
                  ),
                  onPressed: () {
                    setState(() {
                      _passwordVisible = !_passwordVisible;
                    });
                  },
                ),
              ),
              onChanged: (value) {
                setState(() {
                  _password = value;
                });
              },
            ),
          ),
          if (_twoFARequired)
            Padding(
              padding: const EdgeInsets.only(top: 20.0),
              child: TextFormField(
                validator: (value) {
                  if (!_twoFARequired) return null;

                  if (value == null || value.isEmpty) {
                    return "поле не может быть пустым";
                  } else if (value.length != 6) {
                    return "код подтверждения должен содержать 6 цифр";
                  }
                  return null;
                },
                inputFormatters: [
                  FilteringTextInputFormatter.digitsOnly,
                  LengthLimitingTextInputFormatter(6), // Жёсткое ограничение
                ],
                textAlign: TextAlign.center,
                style: const TextStyle(
                  fontSize: 18, // Размер шрифта
                  letterSpacing: 2, // Расстояние между цифрами
                ),
                decoration: const InputDecoration(
                  hintText: "код подтверждения",
                  isDense: true,
                ),
                onChanged: (value) {
                  setState(() {
                    _code = value;
                  });
                },
              ),
            ),
          Padding(
            padding: const EdgeInsets.only(top: 20.0),
            child: Row(
              mainAxisSize: MainAxisSize.max,
              children: [
                Expanded(
                  child: FilledButton(
                    onPressed: _trySendLoginRequest,
                    child: Text("войти"),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
