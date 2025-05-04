import 'package:flutter/material.dart';

class PreviewPicture extends StatelessWidget {
  final String href;
  final bool isArchived;

  const PreviewPicture({
    super.key,
    required this.href,
    required this.isArchived,
  });

  @override
  Widget build(BuildContext context) {
    return Stack(
      alignment: Alignment.bottomLeft,
      children: [
        Container(
          width: 120,
          height: 180,
          decoration: BoxDecoration(
            border: Border.all(color: Colors.black),
            borderRadius: BorderRadius.circular(8),
            image: DecorationImage(
              image: NetworkImage(href),
              fit: BoxFit.cover,
            ),
          ),
        ),
        isArchived
            ? Container(
              width: 120,
              color: Colors.black.withAlpha(65),
              padding: const EdgeInsets.symmetric(horizontal: 4, vertical: 2),
              child: const Text(
                'в архиве',
                style: TextStyle(color: Colors.black, fontSize: 12),
                textAlign: TextAlign.center,
              ),
            )
            : SizedBox.shrink(),
      ],
    );
  }
}
