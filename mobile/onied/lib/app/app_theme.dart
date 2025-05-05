import 'package:flutter/material.dart';

class AppTheme {
  static const Color accentBright = Color(0xFFB34CD9);
  static const Color accentDark = Color(0xFF7A0FAC);
  static const Color accent = Color(0xFF9715D3);
  static const Color backgroundColorHeader = Color(0xFF282828);
  static const Color textColor = Colors.black;
  static const double sidebarWidth = 304;

  static ThemeData get main {
    return ThemeData(
      unselectedWidgetColor: accent,
      primaryColor: Colors.white,
      dividerTheme: DividerThemeData(color: Colors.grey),
      colorScheme: ColorScheme(
        primary: accent,
        secondary: accent,
        brightness: Brightness.light,
        onPrimary: Colors.white,
        onSecondary: Colors.black,
        error: Colors.red,
        onError: Colors.white,
        surface: Colors.white,
        onSurface: textColor,
      ),
      textTheme: const TextTheme(
        bodyLarge: TextStyle(fontSize: 18, fontWeight: FontWeight.w500),
        bodyMedium: TextStyle(fontSize: 16),
      ),
    );
  }

  static ThemeData get dataCourseBlocks {
    return main.copyWith(
      colorScheme: main.colorScheme.copyWith(onSurface: accent),
      textTheme: main.textTheme.copyWith(
        headlineMedium: TextStyle(
          fontSize: 32,
          fontWeight: FontWeight.bold,
          color: textColor,
        ),
        titleLarge: TextStyle(
          fontSize: 24,
          fontWeight: FontWeight.w600,
          color: textColor,
        ),
        bodyLarge: TextStyle(
          fontSize: 18,
          fontWeight: FontWeight.w500,
          color: textColor,
        ),
        bodyMedium: TextStyle(fontSize: 16, color: textColor),
      ),
    );
  }
}
