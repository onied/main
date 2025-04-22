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
      children: [
        // Image
        ClipRRect(
          borderRadius: BorderRadius.circular(32), // 2rem
          child: Image.network(href, width: double.infinity, fit: BoxFit.cover),
        ),

        // Archived label (if applicable)
        if (isArchived)
          Positioned(
            left: 0,
            bottom: 0,
            child: Container(
              width: MediaQuery.of(context).size.width,
              height: 64, // 4rem
              decoration: const BoxDecoration(
                color: Color(0xFF9715D3),
                borderRadius: BorderRadius.vertical(
                  bottom: Radius.circular(32),
                ),
              ),
              alignment: Alignment.center,
              child: const Text(
                'в архиве',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
          ),
      ],
    );
  }
}
