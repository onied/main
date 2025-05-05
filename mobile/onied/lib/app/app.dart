import 'package:onied_mobile/app/app_theme.dart';
import 'package:flutter/material.dart';
import 'package:onied_mobile/views/profile_info/profile_info.dart';
import 'package:onied_mobile/views/purchase/purchase_page.dart';

class MainApp extends StatelessWidget {
  const MainApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: "Onied",
      theme: AppTheme.main,
      home: const PurchasePage(),
      debugShowCheckedModeBanner: false,
    );
  }

}
