import 'video_provider.dart';

class YouTubeVideoProvider extends VideoProvider {
  @override
  RegExp get regex => RegExp(
    r'^((?:https?:)?\/\/)?((?:www|m).)?((?:youtube(-nocookie)?.com|youtu.be))(\/(?:[\w-]+\?v=|embed\/|live\/|v\/)?)(?<videoId>[\w]+)(\S+)?$',
  );

  @override
  String getLink(String href) {
    final matches = regex.firstMatch(href);
    if (matches == null || matches.namedGroup('videoId') == null) {
      throw Exception('Невалидный URL YouTube');
    }

    final videoId = matches.namedGroup('videoId')!;
    return 'https://www.youtube.com/embed/$videoId';
  }
}
