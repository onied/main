import 'package:flutter/material.dart';

class PreviewPicture extends StatelessWidget {
  final double width;
  final double height;
  final String href;
  final bool isArchived;

  const PreviewPicture({
    super.key,
    required this.width,
    required this.height,
    required this.href,
    required this.isArchived,
  });

  @override
  Widget build(BuildContext context) {
    return Stack(
      alignment: Alignment.bottomLeft,
      children: [
        Container(
          width: width,
          height: height,
          decoration: BoxDecoration(
            border: Border.all(color: Colors.black),
            borderRadius: BorderRadius.circular(8),
            image: DecorationImage(
              image: NetworkImage(
                href.endsWith(".svg")
                    ? "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3f/Placeholder_view_vector.svg/330px-Placeholder_view_vector.svg.png"
                    : href,
              ),
              fit: BoxFit.cover,
            ),
          ),
        ),
        isArchived
            ? Container(
              width: width,
              color: Colors.black.withAlpha(128),
              padding: const EdgeInsets.symmetric(horizontal: 4, vertical: 2),
              child: const Text(
                'в архиве',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 12,
                  fontWeight: FontWeight.w500,
                ),
                textAlign: TextAlign.center,
              ),
            )
            : SizedBox.shrink(),
      ],
    );
  }
}
