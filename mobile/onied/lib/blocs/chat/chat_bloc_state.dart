import 'package:equatable/equatable.dart';
import 'package:onied_mobile/models/chat/message.dart';

abstract class ChatBlocState {}

class HistoryLoadingState extends ChatBlocState {}

class HistoryErrorState extends ChatBlocState {
  String errorMessage;

  HistoryErrorState({required this.errorMessage});
}

class HistoryLoadedState extends ChatBlocState with EquatableMixin {
  final List<Message> messageHistory;
  final Object _uniqueId;

  HistoryLoadedState({required this.messageHistory}) : _uniqueId = Object();

  @override
  List<Object?> get props => [_uniqueId];
}
