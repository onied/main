import 'package:flutter/material.dart';

class AuthorBlock extends StatelessWidget {
  final String authorName;
  final String authorAvatarHref;

  const AuthorBlock({
    super.key,
    required this.authorName,
    required this.authorAvatarHref,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          Container(
            width: 40,
            height: 40,
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              image: DecorationImage(
                image: NetworkImage(authorAvatarHref),
                fit: BoxFit.cover,
              ),
            ),
          ),
          const SizedBox(width: 8),
          Text(
            authorName,
            style: const TextStyle(fontSize: 18, fontWeight: FontWeight.w400),
          ),
        ],
      ),
    );
  }
}
