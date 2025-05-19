import 'package:flutter/material.dart';
import 'package:logging/logging.dart';
import 'package:onied_mobile/app/app.dart';
import 'package:onied_mobile/app/injection.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc_event.dart';

void main() {
  Logger.root.level = Level.ALL; // defaults to Level.INFO
  Logger.root.onRecord.listen((record) {
    // ignore: avoid_print
    print('${record.level.name}: ${record.time}: ${record.message}');
  });

  WidgetsFlutterBinding.ensureInitialized();
  setupDependencies();

  runApp(const MainApp());
}
