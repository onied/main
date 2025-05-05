import 'package:flutter/material.dart';
import 'package:mobile_authorization/ThemeDataAuthorization.dart';

class FormDivider extends StatelessWidget {
  final String text;
  const FormDivider({super.key, this.text = "или"});

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        const Expanded(
          child: Divider(
            height: 1,
            color: AppTheme.textFieldBorder,
            indent: 20,
            endIndent: 5,
          ),
        ),
        Text(
          text,
          style: TextStyle(
              fontSize: 12,
              color: AppTheme.textSecondaryColor
          ),
        ),
        const Expanded(
          child: Divider(
            height: 1,
            color: AppTheme.textFieldBorder,
            indent: 5,
            endIndent: 20,
          ),
        )
      ],
    );
  }

}
