import 'package:onied_mobile/models/chat/message.dart';

abstract class ChatBlocEvent {}

class LoadHistory extends ChatBlocEvent {}

class UpdateHistory extends ChatBlocEvent {
  List<Message> newHistory;

  UpdateHistory({required this.newHistory});
}
