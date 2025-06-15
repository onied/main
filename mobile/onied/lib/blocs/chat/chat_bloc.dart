import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/blocs/chat/chat_bloc_event.dart';
import 'package:onied_mobile/blocs/chat/chat_bloc_state.dart';
import 'package:onied_mobile/models/chat/message.dart';
import 'package:onied_mobile/repositories/chat_repository.dart';

class ChatBloc extends Bloc<ChatBlocEvent, ChatBlocState> {
  final ChatRepository chatRepository;

  ChatBloc({required this.chatRepository}) : super(HistoryLoadingState()) {
    on<LoadHistory>(_onLoadHistory);
    on<UpdateHistory>(_onUpdateHistory);
  }

  Future<void> _onLoadHistory(
    LoadHistory event,
    Emitter<ChatBlocState> emit,
  ) async {
    // final defaultTimestamp = Timestamp.fromDateTime(
    //   DateTime(1971, 1, 1, 12, 0),
    // );
    // final defaultMessage = MessageResponse(
    //   messageId: "id1",
    //   supportNumber: 20,
    //   createdAt: defaultTimestamp,
    //   message: "message message",
    //   isSystem: true,
    // );
    // final meDefaultMessage = MessageResponse(
    //   messageId: "id2",
    //   supportNumber: null,
    //   createdAt: defaultTimestamp,
    //   message: "me message",
    //   isSystem: false,
    // );
    // final systemMessage = MessageResponse(
    //   messageId: "id3",
    //   supportNumber: null,
    //   createdAt: defaultTimestamp,
    //   message: "close-session",
    //   isSystem: true,
    // );
    // final defaultReadAtResponse = ReadAtResponse(
    //   messageId: meDefaultMessage.messageId,
    //   readAt: defaultTimestamp,
    // );

    // final readItem = Message.fromGrpc(meDefaultMessage)
    //   ..readAt = defaultReadAtResponse.readAt.toDateTime();

    // emit(
    //   HistoryLoadedState(
    //     messageHistory: [
    //       Message.fromGrpc(defaultMessage),
    //       readItem,
    //       Message.fromGrpc(systemMessage),
    //       Message.fromGrpc(defaultMessage),
    //       Message.fromGrpc(defaultMessage),
    //       Message.fromGrpc(defaultMessage),
    //       Message.fromGrpc(defaultMessage),
    //       Message.fromGrpc(defaultMessage),
    //       Message.fromGrpc(defaultMessage),
    //       Message.fromGrpc(defaultMessage),
    //     ],
    //   ),
    // );
    // TODO: раскомментировать
    emit(
      HistoryLoadedState(messageHistory: await chatRepository.getMessages()),
    );
    chatRepository.messageStream.listen((event) async {
      final history = List<Message>.from(
        (state as HistoryLoadedState).messageHistory..add(event),
      );
      emit(HistoryLoadedState(messageHistory: history));
    });
    chatRepository.readAtStream.listen((event) async {
      final history = List<Message>.from(
        (state as HistoryLoadedState).messageHistory,
      );
      for (var msg in history) {
        if (msg.messageId == event.messageId) {
          msg.readAt = event.readAt.toDateTime(toLocal: true);
          break;
        }
      }
      emit(HistoryLoadedState(messageHistory: history));
    });
  }

  Future<void> _onUpdateHistory(
    UpdateHistory event,
    Emitter<ChatBlocState> emit,
  ) async {
    emit(
      HistoryLoadedState(messageHistory: List<Message>.from(event.newHistory)),
    );
  }
}
