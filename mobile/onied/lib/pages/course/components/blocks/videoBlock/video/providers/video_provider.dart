import 'dart:core';

abstract class VideoProvider {
  RegExp get regex;

  String getLink(String href);
}
