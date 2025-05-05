import 'package:go_router/go_router.dart';
import 'package:onied_mobile/components/button/button.dart';
import 'package:onied_mobile/components/picture_preview/picture_preview.dart';
import 'package:onied_mobile/components/search_bar/search_bar.dart';
import 'package:onied_mobile/repositories/repository_mock.dart';
import 'components/allow_certificates.dart';
import 'components/author_block.dart';
import 'components/course_program.dart';
import 'package:flutter/material.dart';
import 'package:onied_mobile/models/course_preview_dto.dart';

class PreviewPage extends StatefulWidget {
  final String courseId;

  const PreviewPage({super.key, required this.courseId});

  @override
  State<PreviewPage> createState() => _PreviewPageState();
}

class _PreviewPageState extends State<PreviewPage> {
  late PreviewDto courseDto;

  @override
  void initState() {
    super.initState();
    courseDto = MockPreviewRepository().getCourseById(widget.courseId);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: const CourseSearchBar(),
      body: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                PreviewPicture(
                  width: 120,
                  height: 180,
                  href: courseDto.pictureHref,
                  isArchived: courseDto.isArchived,
                ),
                const SizedBox(width: 16),
                Expanded(
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        courseDto.title,
                        style: TextStyle(
                          fontSize: 18,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      SizedBox(height: 4),
                      AuthorBlock(
                        authorName: courseDto.courseAuthor.name,
                        authorAvatarHref: courseDto.courseAuthor.avatarHref,
                      ),
                      SizedBox(height: 4),
                      Text(
                        courseDto.category.name,
                        style: TextStyle(fontSize: 14),
                      ),
                      courseDto.hasCertificates
                          ? AllowCertificate()
                          : SizedBox.shrink(),
                    ],
                  ),
                ),
              ],
            ),
            Container(
              width: double.infinity,
              padding: EdgeInsets.symmetric(vertical: 16),
              child: Button(
                text: courseDto.isOwned ? "продолжить" : "купить",
                onPressed: () {
                  if (courseDto.isOwned) {
                    context.push("/course/${courseDto.id}/learn");
                  } else {
                    context.push("/purchase");
                  }
                },
              ),
            ),
            // Description
            Text(
              courseDto.description,
              style: TextStyle(fontSize: 16, height: 1.5),
            ),
            Padding(padding: EdgeInsets.only(bottom: 10)),
            CourseProgram(modules: courseDto.courseProgram),
          ],
        ),
      ),
    );
  }
}
