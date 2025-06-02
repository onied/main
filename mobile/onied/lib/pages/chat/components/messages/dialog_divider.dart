import 'package:flutter/material.dart';

class DialogDivider extends StatelessWidget {
  final String text;

  const DialogDivider({super.key, required this.text});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 16.0),
      child: Row(
        children: [
          Expanded(
            child: Divider(color: Colors.grey[400], thickness: 1, height: 1),
          ),
          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 12.0),
            child: Text(
              text,
              style: TextStyle(color: Colors.grey[600], fontSize: 14),
            ),
          ),
          Expanded(
            child: Divider(color: Colors.grey[400], thickness: 1, height: 1),
          ),
        ],
      ),
    );
  }
}
