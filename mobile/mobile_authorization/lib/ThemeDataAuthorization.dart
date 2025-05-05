import 'package:flutter/material.dart';

class AppTheme {
  static const Color accentBright = Color(0xFFB34CD9);
  static const Color accentDark = Color(0xFF7A0FAC);
  static const Color accent = Color(0xFF9715D3);
  static const Color backgroundColorHeader = Color(0xFF282828);
  static const Color textColor = Colors.black;
  static const double sidebarWidth = 304;

  static const Color textSecondaryColor = Color(0xFF737373);

  static const Color textFieldBackground = Color(0xFFFAFAFA);
  static const Color textFieldBorder = Color(0xFFDBDBDB);

  static ThemeData get themeDataAuthorization {
    return ThemeData(
        fontFamily: "Inter",
        unselectedWidgetColor: accent,
        primaryColor: Colors.white,
        colorScheme: ColorScheme(
          primary: accent,
          secondary: accent,
          brightness: Brightness.light,
          onPrimary: Colors.white,
          onSecondary: Colors.black,
          error: Colors.red,
          onError: Colors.white,
          surface: Colors.white,
          onSurface: accent,
        ),
        textTheme: const TextTheme(
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
            bodyMedium: TextStyle(
                fontSize: 16,
                color: textColor
            )
        ),
        inputDecorationTheme: const InputDecorationTheme(
            border: OutlineInputBorder(
                borderSide: BorderSide(color: textFieldBorder),
                borderRadius: BorderRadius.all(Radius.circular(10.0))
            ),
            enabledBorder: OutlineInputBorder(
                borderSide: BorderSide(color: textFieldBorder),
                borderRadius: BorderRadius.all(Radius.circular(10.0))
            ),
            focusedBorder: OutlineInputBorder(
                borderSide: BorderSide(
                    color: accentDark,
                    width: 2
                ),
                borderRadius: BorderRadius.all(Radius.circular(10.0))
            ),
            hintStyle: TextStyle(
              fontSize: 16,
              color: textSecondaryColor,
            ),
            filled: true,
            fillColor: textFieldBackground
        ),
        filledButtonTheme: FilledButtonThemeData(
            style: FilledButton.styleFrom(
              backgroundColor: accentBright,
              shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(10.0)
              ),
              textStyle: TextStyle(
                  fontSize: 24,
                  fontWeight: FontWeight.bold
              ),
              padding: EdgeInsets.symmetric(horizontal: 20, vertical: 12),
              elevation: 5,
              shadowColor: accentBright.withValues(alpha: 0.5),
            )
        )
    );
  }
}
