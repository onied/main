import 'package:flutter/material.dart';
import 'package:onied_mobile/models/chat/message.dart';
import 'package:onied_mobile/pages/chat/components/messages/dialog_divider.dart';
import 'package:onied_mobile/pages/chat/components/messages/message_container.dart';

class MessageBuilder {
  static StatelessWidget build(Message msg) {
    if (msg.isSystem && msg.message.startsWith("close-session")) {
      return DialogDivider(text: "Диалог завершен");
    } else {
      return MessageContainer(message: msg);
    }
  }
}
