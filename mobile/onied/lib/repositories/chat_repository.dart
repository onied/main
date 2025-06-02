import 'dart:convert';
import 'dart:io';

import 'package:grpc/grpc.dart';
import 'package:http/http.dart' as http;
import 'package:onied_mobile/app/config.dart';
import 'package:onied_mobile/models/chat/message.dart';
import 'package:onied_mobile/protos/generated/chat.pbgrpc.dart' as ChatGrpc;
import 'package:onied_mobile/protos/helpers/auth_interceptor.dart';
import 'package:onied_mobile/providers/authorization_provider.dart';

class ChatRepository {
  final AuthorizationProvider authorizationProvider;

  late ClientChannel _supportChannel;
  late ChatGrpc.UserChatServiceClient _chatStub;

  ChatRepository({required this.authorizationProvider});

  Future<List<Message>> getMessages() async {
    final credentials = await authorizationProvider.getCredentials();
    if (credentials == null) {
      throw Exception("grpc connection cannot work without authentication");
    }
    final response = await http.get(
      Uri.parse("${Config.backendUrl}/chat"),
      headers: {"Authorization": 'Bearer ' + credentials.accessToken},
    );

    if (response.statusCode != HttpStatus.ok) {
      return [];
    }
    final messages = jsonDecode(response.body)["messages"] as List;
    return messages.map((json) => Message.fromJson(json)).toList();
  }

  Future<void> connect() async {
    final credentials = await authorizationProvider.getCredentials();
    if (credentials == null) {
      throw Exception("grpc connection cannot work without authentication");
    }

    _supportChannel = ClientChannel(
      Config.ChatGrpcHost,
      port: Config.ChatGrpcPort,
      options: ChannelOptions(
        credentials: ChannelCredentials.secure(
          onBadCertificate: (X509Certificate cert, String host) {
            // Всегда принимать любой сертификат
            return true;
          },
        ),
      ),
    );
    _chatStub = ChatGrpc.UserChatServiceClient(
      _supportChannel,
      interceptors: [
        AuthInterceptor({'Authorization': "Bearer ${credentials.accessToken}"}),
      ],
    );
  }

  Stream<Message> get messageStream => _chatStub
      .receiveMessage(ChatGrpc.Empty())
      .map((msg) => Message.fromGrpc(msg));

  Stream<ChatGrpc.ReadAtResponse> get readAtStream =>
      _chatStub.receiveReadAt(ChatGrpc.Empty());

  Future<void> sendMessage(ChatGrpc.SendMessageRequest request) async =>
      await _chatStub.sendMessage(request);

  Future<void> markMessageAsRead(
    ChatGrpc.MarkMessageAsReadRequest request,
  ) async => await _chatStub.markMessageAsRead(request);

  Future<void> shutdown() async => await _supportChannel.shutdown();
}
