import 'package:collection/collection.dart';
import 'package:onied_mobile/shared/video/providers/rutube_video_provider.dart';
import 'package:onied_mobile/shared/video/providers/video_provider.dart';
import 'package:onied_mobile/shared/video/providers/vk_video_provider.dart';
import 'package:onied_mobile/shared/video/providers/youtube_video_provider.dart';
import 'package:flutter/material.dart';
import 'package:webview_flutter/webview_flutter.dart';

class VideoWebView extends StatefulWidget {
  final String videoUrl;

  const VideoWebView({super.key, required this.videoUrl});

  @override
  State<VideoWebView> createState() => _VideoWebViewState();
}

class _VideoWebViewState extends State<VideoWebView> {
  late WebViewController _controller;
  bool _hasError = false;
  bool _isInitialized = false;

  @override
  void initState() {
    super.initState();
    _initializeController(widget.videoUrl);
  }

  @override
  void didUpdateWidget(covariant VideoWebView oldWidget) {
    super.didUpdateWidget(oldWidget);
    setState(() {
      _hasError = false;
    });
    if (widget.videoUrl != oldWidget.videoUrl) {
      _initializeController(widget.videoUrl);
    }
  }

  Future<void> _initializeController(String videoUrl) async {
    try {
      final provider = embedElements.firstWhereOrNull(
        (provider) => provider.regex.hasMatch(videoUrl)
      );
      if (provider == null){
        throw Exception('Неверный формат ссылки на видео');
      }
      final embedUrl = provider.getLink(videoUrl);
      _controller = WebViewController()
        ..setJavaScriptMode(JavaScriptMode.unrestricted)
        ..loadRequest(Uri.parse(embedUrl));
      setState(() {
        _isInitialized = true;
        _hasError = false;
      });
    } catch (error) {
      setState(() {
        _isInitialized = true;
        _hasError = true;
      });
    }
  }

  final List<VideoProvider> embedElements = [
    RutubeVideoProvider(),
    YouTubeVideoProvider(),
    VkVideoProvider(),
  ];

  @override
  Widget build(BuildContext context) {
    if (!_isInitialized) {
      return Center(child: CircularProgressIndicator());
    } else if (_hasError) {
      return Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(Icons.error, size: 50, color: Colors.red),
            const SizedBox(height: 16),
            Text(
              'Ошибка загрузки видео',
              style: Theme.of(context).textTheme.titleLarge,
            ),
          ],
        ),
      );
    }
    return WebViewWidget(controller: _controller);
  }
}
