import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:logging/logging.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/models/enums/sex.dart';
import 'package:onied_mobile/models/registration_form_data.dart';

class RegistrationForm extends StatefulWidget {
  const RegistrationForm({super.key});

  @override
  State<StatefulWidget> createState() => _RegistrationState();
}

class _RegistrationState extends State<RegistrationForm> {
  final _formKey = GlobalKey<FormState>();
  final _logger = Logger("LoginFormState");

  late String _name;
  late String _surname;
  Sex _sex = Sex.noSex;
  late String _email;
  late String _password;
  bool _passwordVisible = false;

  Future<void> _trySendRegisterRequest() async {
    if (!_formKey.currentState!.validate()) {
      return;
    }

    ScaffoldMessenger.of(
      context,
    ).showSnackBar(const SnackBar(content: Text('Запрос к серверу...')));
    final formDate = RegistrationFormData(
      name: _name,
      surname: _surname,
      sex: _sex.index,
      email: _email,
      password: _password,
    );
    _logger.log(
      Level.INFO,
      "Trying to register ${jsonEncode(formDate.toJson())}...",
    );
    // final authData = AuthorizationApi.login(formDate);
    context.go("/");
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
              }
              return null;
            },
            decoration: const InputDecoration(hintText: "Имя"),
            onChanged: (value) {
              setState(() {
                _name = value;
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
              decoration: const InputDecoration(hintText: "Фамилия"),
              onChanged: (value) {
                setState(() {
                  _surname = value;
                });
              },
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(top: 16, left: 4),
            child: SizedBox(
              width: double.infinity,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Padding(
                    padding: EdgeInsets.only(right: 8),
                    child: Text(
                      "Пол:",
                      style: Theme.of(context).textTheme.bodyMedium,
                    ),
                  ),
                  _buildRadioOption(Sex.noSex, 'не указано'),
                  _buildRadioOption(Sex.man, 'мужской'),
                  _buildRadioOption(Sex.woman, 'женский'),
                ],
              ),
            ),
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
              decoration: const InputDecoration(hintText: "Эл. почта"),
              onChanged: (value) {
                setState(() {
                  _email = value;
                });
              },
            ),
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
          Padding(
            padding: const EdgeInsets.only(top: 10.0),
            child: TextFormField(
              validator: (value) {
                if (value == null || value.isEmpty) {
                  return "поле не может быть пустым";
                } else if (value != _password) {
                  return "пароли не совпадают";
                }
                return null;
              },
              obscureText: !_passwordVisible,
              decoration: InputDecoration(hintText: "повторите пароль"),
            ),
          ),
          Padding(
            padding: const EdgeInsets.only(top: 20.0),
            child: Row(
              mainAxisSize: MainAxisSize.max,
              children: [
                Expanded(
                  child: FilledButton(
                    onPressed: _trySendRegisterRequest,
                    child: const Text("зарегистрироваться"),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildRadioOption(Sex value, String text) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        Radio<Sex>(
          value: value,
          groupValue: _sex,
          onChanged: (v) => setState(() => _sex = v!),
          materialTapTargetSize: MaterialTapTargetSize.shrinkWrap,
        ),
        Text(text, style: TextStyle(fontSize: 14)),
        SizedBox(width: 8), // Отступ между вариантами
      ],
    );
  }
}
