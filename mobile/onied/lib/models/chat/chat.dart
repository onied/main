import 'package:onied_mobile/models/chat/message.dart';

class Chat {
  final int? supportNumber;
  final String? currentSessionId;
  final List<Message> messages;

  Chat({this.supportNumber, this.currentSessionId, required this.messages});

  factory Chat.fromJson(Map<String, dynamic> json) {
    return Chat(
      supportNumber: json['supportNumber'],
      currentSessionId: json['currentSessionId'],
      messages:
          (json['messages'] as List)
              .map((messageJson) => Message.fromJson(messageJson))
              .toList(),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'supportNumber': supportNumber,
      'currentSessionId': currentSessionId,
      'messages': messages.map((message) => message.toJson()).toList(),
    };
  }
}
