import 'package:flutter/material.dart';

import 'package:logging/logging.dart';

import 'package:mobile_authorization/ThemeDataAuthorization.dart';
import 'package:mobile_authorization/presenters/authorization_page.dart';
import 'package:mobile_authorization/presenters/registration_page.dart';


Future<void> main() async {
  Logger.root.level = Level.ALL; // defaults to Level.INFO
  Logger.root.onRecord.listen((record) {
    print('${record.level.name}: ${record.time}: ${record.message}');
  });

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Authorization',
      theme: AppTheme.themeDataAuthorization,
      initialRoute: "/login",
      routes: {
        "/login": (context) => const AuthorizationPage(),
        "/register": (context) => const RegistrationPage()
      },
    );
  }
}
