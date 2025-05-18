import 'video_provider.dart';

class RutubeVideoProvider extends VideoProvider {
  @override
  RegExp get regex => RegExp(
    r'^((?:https?:)?\/\/)?(rutube\.ru)(\/video)(?<videoId>\/[\w\d]+)?(\/[\S]+)?$',
  );

  @override
  String getLink(String href) {
    final matches = regex.firstMatch(href);
    if (matches == null || matches.namedGroup('videoId') == null) {
      throw Exception('Невалидный URL Rutube');
    }

    final videoId = matches.namedGroup('videoId')!;
    return 'https://rutube.ru/play/embed$videoId';
  }
}
