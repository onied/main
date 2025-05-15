import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/components/button/button.dart';
import 'package:onied_mobile/components/picture_preview/picture_preview.dart';
import 'package:onied_mobile/models/course_card_model.dart';

class CourseCard extends StatelessWidget {
  final CourseCardModel courseCard;

  const CourseCard({super.key, required this.courseCard});

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: () => context.push("/course/${courseCard.id}"),
      child: Card(
        margin: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
        shadowColor: Colors.black,
        elevation: 20,
        child: Padding(
          padding: const EdgeInsets.all(12),
          child: Row(
            children: [
              PreviewPicture(
                width: 100,
                height: 140,
                href: courseCard.imageUrl,
                isArchived: courseCard.isArchived,
              ),
              const SizedBox(width: 12),
              Expanded(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      courseCard.title,
                      style: const TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                    Text(
                      'Категория: ${courseCard.category}',
                      style: const TextStyle(fontSize: 12),
                    ),
                    Text(
                      courseCard.description,
                      maxLines: 3,
                      overflow: TextOverflow.ellipsis,
                      style: const TextStyle(fontSize: 12),
                    ),
                    const SizedBox(height: 4),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        if (courseCard.price > 0)
                          Text('${courseCard.price}₽')
                        else
                          const Text('Бесплатно'),
                        Button(
                          onPressed: () {},
                          text: courseCard.isOwned ? 'продолжить' : 'купить',
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
