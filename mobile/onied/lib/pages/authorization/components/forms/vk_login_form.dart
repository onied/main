import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_svg/svg.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc.dart';
import 'package:onied_mobile/blocs/authorization/authorization_bloc_event.dart';

class VkLoginForm extends StatefulWidget {
  const VkLoginForm({super.key});

  @override
  State<StatefulWidget> createState() => _VkLoginState();
}

class _VkLoginState extends State<StatefulWidget> {
  String token = "";

  @override
  Widget build(BuildContext context) {
    return TextButton(
      onPressed: () {
        context.read<AuthorizationBloc>().add(LoginWithVk());
      },
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
