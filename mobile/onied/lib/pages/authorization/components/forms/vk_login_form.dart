import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:logging/logging.dart';
import 'package:onied_mobile/providers/vk_auth_service.dart';

class VkLoginForm extends StatefulWidget {
  const VkLoginForm({super.key});

  @override
  State<StatefulWidget> createState() => _VkLoginState();
}

class _VkLoginState extends State<StatefulWidget> {
  final _logger = Logger("_VkLoginState");
  final _vkAuthService = VKAuthService();

  String token = "";

  Future<void> _tryLogin() async {
    final code = await _vkAuthService.authViaVK();
    if (code == null) return;
    _logger.log(Level.INFO, "Got Code! $code");
    // AuthorizationApi.loginVk(
    //     LoginVkFormData(code: code, redirectUri: Config.redirectUri));
  }

  @override
  Widget build(BuildContext context) {
    // TODO: implement build
    return TextButton(
      onPressed: _tryLogin,
      style: TextButton.styleFrom(textStyle: TextStyle(color: Colors.black)),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          SvgPicture.asset(
            'assets/icons/vk.svg',
            width: 28, // specify the desired size
            height: 28,
          ),
          Padding(
            padding: EdgeInsets.only(left: 10.0),
            child: Text(
              "войти через VK ID",
              style: TextStyle(
                fontWeight: FontWeight.w300,
                fontSize: 24,
                color: Colors.black,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
