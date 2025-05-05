import 'package:design_courses/data/repository_mock.dart';
import 'package:design_courses/widgets/course_preview/allow_certificates.dart';
import 'package:design_courses/widgets/course_preview/author_block.dart';
import 'package:design_courses/widgets/course_preview/course_program.dart';
import 'package:design_courses/widgets/shared/picture_preview.dart';
import 'package:flutter/material.dart';
import 'package:design_courses/widgets/shared/button.dart';
import 'package:design_courses/widgets/shared/app_bar.dart';
import 'package:design_courses/data/course_preview_dto.dart';

class PreviewPage extends StatefulWidget {
  final int courseId;

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
      appBar: const CustomAppBar(),
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
                textButton: courseDto.isOwned ? "продолжить" : "купить",
                onPressed: () {},
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
