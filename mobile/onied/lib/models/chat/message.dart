import 'package:onied_mobile/protos/generated/chat.pb.dart';

class Message {
  final String messageId;
  final int? supportNumber;
  final DateTime createdAt;
  final String message;
  final bool isSystem;
  DateTime? readAt;
  final List<MessageFile> files;

  Message({
    required this.messageId,
    this.supportNumber,
    required this.createdAt,
    required this.message,
    required this.isSystem,
    this.readAt,
    required this.files,
  });

  factory Message.fromJson(Map<String, dynamic> json) {
    return Message(
      messageId: json['messageId'],
      supportNumber: json['supportNumber'],
      createdAt: DateTime.parse(json['createdAt']),
      message: json['message'],
      isSystem: json['isSystem'],
      readAt: json['readAt'] != null ? DateTime.parse(json['readAt']) : null,
      files:
          (json['files'] as List)
              .map((fileJson) => MessageFile.fromJson(fileJson))
              .toList(),
    );
  }

  factory Message.fromGrpc(MessageResponse response) {
    return Message(
      messageId: response.messageId,
      supportNumber: response.supportNumber,
      createdAt: response.createdAt.toDateTime(),
      message: response.message,
      isSystem: response.isSystem,
      files: [],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'messageId': messageId,
      'supportNumber': supportNumber,
      'createdAt': createdAt.toIso8601String(),
      'message': message,
      'isSystem': isSystem,
      'readAt': readAt?.toIso8601String(),
      'files': files.map((file) => file.toJson()).toList(),
    };
  }
}

class MessageFile {
  final String filename;
  final String fileUrl;

  MessageFile({required this.filename, required this.fileUrl});

  factory MessageFile.fromJson(Map<String, dynamic> json) {
    return MessageFile(filename: json['filename'], fileUrl: json['fileUrl']);
  }

  Map<String, dynamic> toJson() {
    return {'filename': filename, 'fileUrl': fileUrl};
  }
}
