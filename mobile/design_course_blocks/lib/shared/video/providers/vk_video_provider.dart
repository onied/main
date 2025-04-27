import 'package:design_course_blocks/shared/video/providers/video_provider.dart';

class VkVideoProvider extends VideoProvider {
  @override
  RegExp get regex => RegExp(
      r'^(?:https:\/\/)?(?:www\.)?(?:vk\.com|m\.vk\.com|vkvideo\.ru)(?:\/video|video-)(?<videoOid>[\d-]+)_(?<videoId>[\d]+)$');

  @override
  String getLink(String href) {
    final matches = regex.firstMatch(href);
    if (matches == null ||
        matches.namedGroup('videoOid') == null ||
        matches.namedGroup('videoId') == null) {
      throw Exception('Невалидный URL VK');
    }

    final videoOid = matches.namedGroup('videoOid')!;
    final videoId = matches.namedGroup('videoId')!;
    return 'https://vk.com/video_ext.php?oid=$videoOid&id=$videoId';
  }
}
