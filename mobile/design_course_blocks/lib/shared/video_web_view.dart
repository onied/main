import 'package:flutter/material.dart';
import 'package:webview_flutter/webview_flutter.dart';

class VideoWebView extends StatefulWidget {
  final String videoUrl;

  const VideoWebView({super.key, required this.videoUrl});

  @override
  State<VideoWebView> createState() => _VideoWebViewState();
}

class _VideoWebViewState extends State<VideoWebView> {
  late final WebViewController _controller;
  bool _hasError = false;

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
    final Uri? parsedUrl = Uri.tryParse(videoUrl);
    if (parsedUrl == null || !parsedUrl.hasScheme) {
      setState(() {
        _hasError = true;
      });
      return;
    }
    _controller = WebViewController()
      ..setJavaScriptMode(JavaScriptMode.unrestricted)
      ..loadRequest(parsedUrl);
    _controller.reload();
    setState(() {
      _hasError = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    if (_hasError) {
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
