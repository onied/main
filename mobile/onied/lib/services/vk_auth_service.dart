import 'package:flutter_web_auth_2/flutter_web_auth_2.dart';
import 'package:logging/logging.dart';
import 'package:onied_mobile/app/config.dart';

class VKAuthService {
  final _logger = Logger("VKAuthService");

  Future<String?> authViaVK() async {
    try {
      final result = await FlutterWebAuth2.authenticate(
        url: Config.authUrl,
        callbackUrlScheme: 'onied',
      );

      final uri = Uri.parse(result);
      return uri.queryParameters['code'];
    } catch (e) {
      _logger.log(Level.SHOUT, 'Auth error: $e');
      return null;
    }
  }
}
