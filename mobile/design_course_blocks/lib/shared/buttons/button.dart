import 'package:flutter/material.dart';

import '../../app/app_theme.dart';

class Button extends StatelessWidget {
  final String textButton;
  final VoidCallback? onPressed;

  const Button({
    super.key,
    required this.textButton,
    required this.onPressed
  });

  @override
  Widget build(BuildContext context) {
    return ElevatedButton(
      style: ButtonStyle(
        backgroundColor: WidgetStateProperty.resolveWith<Color>(
              (Set<WidgetState> states) {
            if (states.contains(WidgetState.pressed)) {
              return AppTheme.accentDark;
            }
            if (states.contains(WidgetState.hovered)) {
              return AppTheme.accentBright;
            }
            if (states.contains(WidgetState.disabled)) {
              return Colors.grey;
            }
            return AppTheme.accent;
          },
        ),
      ),
      onPressed: onPressed,
      child: Text(
        textButton,
        style: Theme.of(context).textTheme.bodyLarge?.copyWith(color: Colors.white),
      ),
    );
  }

}
