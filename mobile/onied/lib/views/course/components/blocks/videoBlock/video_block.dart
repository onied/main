import 'video/video_web_view.dart';
import 'package:flutter/material.dart';

class VideoBlock extends StatelessWidget {
  final dynamic block;

  const VideoBlock({
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
        const SizedBox(height: 40,),
        Center(
          child: Container(
            width: MediaQuery.of(context).size.width * 0.6,
            constraints: const BoxConstraints(minWidth: 480),
            child: AspectRatio(
              aspectRatio: 16 / 9,
              child: VideoWebView(
                videoUrl: block['href'] ?? '',
              ),
            ),
          ),
        ),
      ],
    );
  }

}
