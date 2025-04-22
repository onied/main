import 'package:flutter/material.dart';
//import 'package:flutter_markdown/flutter_markdown.dart';

import '../data/repository_mock.dart';
import '../data/course_preview_dto.dart';
import '../widgets/course_preview/picture_preview.dart';
import '../widgets/course_preview/author_block.dart';
import '../widgets/course_preview/allow_certificates.dart';
import '../widgets/course_preview/course_program.dart';

class PreviewPage extends StatelessWidget {
  final PreviewDto dto = MockPreviewRepository().getSampleCourse();

  PreviewPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(dto.title)),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Expanded(
                    flex: 3,
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          dto.title,
                          style: const TextStyle(
                            fontSize: 24,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        const SizedBox(height: 8),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            Text(
                              'Категория: ${dto.category.name}',
                              style: const TextStyle(color: Colors.grey),
                            ),
                            Text(
                              'Время прохождения: ${dto.hoursCount} ч.',
                              style: const TextStyle(color: Colors.grey),
                            ),
                          ],
                        ),
                        const SizedBox(height: 16),
                        Text(dto.description),
                        if (dto.courseProgram != null) ...[
                          const SizedBox(height: 16),
                          CourseProgram(modules: dto.courseProgram!),
                        ],
                      ],
                    ),
                  ),
                  const SizedBox(width: 16),
                  Expanded(
                    flex: 2,
                    child: Column(
                      children: [
                        PreviewPicture(
                          href: dto.pictureHref,
                          isArchived: dto.isArchived,
                        ),
                        if (dto.price > 0)
                          Padding(
                            padding: const EdgeInsets.symmetric(vertical: 12.0),
                            child: Text(
                              '${dto.price} ₽',
                              style: const TextStyle(
                                fontSize: 22,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                          ),
                        ElevatedButton(
                          onPressed: () {},
                          style: ElevatedButton.styleFrom(
                            backgroundColor:
                                dto.isOwned || dto.price == 0
                                    ? Colors.green
                                    : null,
                            minimumSize: const Size.fromHeight(50),
                          ),
                          child: Text(
                            dto.isOwned
                                ? 'Продолжить'
                                : dto.price > 0
                                ? 'Купить'
                                : 'Начать',
                            style: const TextStyle(fontSize: 18),
                          ),
                        ),
                        const SizedBox(height: 16),
                        AuthorBlock(
                          authorName: dto.courseAuthor.name,
                          authorAvatarHref: dto.courseAuthor.avatarHref,
                        ),
                        if (dto.hasCertificates) ...[
                          const SizedBox(height: 16),
                          const AllowCertificate(),
                        ],
                      ],
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
    );
  }
}
