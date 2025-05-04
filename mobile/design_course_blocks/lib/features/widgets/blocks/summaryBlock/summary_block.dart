import 'package:flutter/material.dart';
import 'package:flutter_markdown/flutter_markdown.dart';

import '../../../../app/app_theme.dart';

class SummaryBlock extends StatelessWidget {
  final dynamic block;

  const SummaryBlock({
    super.key,
    required this.block,
  });


  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          block['title'],
          style: Theme.of(context).textTheme.headlineMedium,
        ),
        const SizedBox(height: 16,),
        MarkdownBody(
          data: block['markdownText'] ?? '',
          styleSheet: MarkdownStyleSheet(
            p: Theme.of(context).textTheme.bodyLarge
          ),
        ),
        if (block['fileName'] != null && block['fileHref'] != null) ...[
          const SizedBox(height: 16,),
          Row(
            children: [
              Text(
                "Файл:",
                style: TextStyle(
                  color: Colors.black,
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                ),
              ),
              TextButton(
                onPressed: () {},
                child: Row(
                  children: [
                    Icon(
                      Icons.insert_drive_file,
                      color: AppTheme.accent,
                      size: 24,
                    ),
                    Text(
                      block['fileName'],
                      style: const TextStyle(
                        color: AppTheme.accent,
                        fontSize: 24,
                        fontWeight: FontWeight.bold,
                        decoration: TextDecoration.underline,
                        decorationColor: AppTheme.accent
                      ),
                    ),
                  ],
                )
              ),
            ],
          ),
        ]
      ],
    );
  }

}
