import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/app/injection.dart';
import 'package:onied_mobile/blocs/chat/chat_bloc.dart';
import 'package:onied_mobile/blocs/chat/chat_bloc_event.dart';
import 'package:onied_mobile/blocs/chat/chat_bloc_state.dart';
import 'package:onied_mobile/models/chat/message.dart';
import 'package:onied_mobile/pages/chat/components/inputs/message_input_panel.dart';
import 'package:onied_mobile/pages/chat/components/messages/message_builder.dart';
import 'package:onied_mobile/repositories/chat_repository.dart';

class ChatPage extends StatefulWidget {
  const ChatPage({super.key});

  @override
  State<ChatPage> createState() => _ChatPageState();
}

class _ChatPageState extends State<ChatPage> {
  @override
  Widget build(BuildContext context) {
    return FutureBuilder(
      future: getIt<ChatRepository>().connect(),
      builder: (context, state) {
        return BlocProvider(
          create:
              (context) =>
                  ChatBloc(chatRepository: getIt<ChatRepository>())
                    ..add(LoadHistory()),
          child: BlocBuilder<ChatBloc, ChatBlocState>(
            builder: (context, state) {
              return switch (state) {
                HistoryLoadingState() => const Center(
                  child: CircularProgressIndicator(),
                ),
                HistoryLoadedState(:final messageHistory) => Scaffold(
                  appBar: AppBar(
                    centerTitle: true,
                    title: Text(
                      "Поддержка",
                      style: Theme.of(
                        context,
                      ).textTheme.titleLarge?.copyWith(color: Colors.white),
                    ),
                    backgroundColor: AppTheme.backgroundColorHeader,
                    foregroundColor: AppTheme.accent,
                  ),

                  body: Column(
                    children: [
                      Expanded(
                        child: Padding(
                          padding: EdgeInsets.all(16),
                          child: SingleChildScrollView(
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.stretch,
                              spacing: 16,
                              children:
                                  messageHistory
                                      .map((msg) => MessageBuilder.build(msg))
                                      .toList(),
                            ),
                          ),
                        ),
                      ),
                      MessageInputPanel(
                        onSendMessage: (message) async {
                          final chatBloc = context.read<ChatBloc>();
                          final state = chatBloc.state as HistoryLoadedState;

                          // try {
                          //   await chatBloc.chatRepository.sendMessage(
                          //     SendMessageRequest(messageContent: message),
                          //   );
                          // } on Exception {
                          //   //
                          // }
                          // TODO: убрать при слиянии
                          final newMessage = Message(
                            messageId: "id-new-message",
                            supportNumber: 0,
                            createdAt: DateTime.now(),
                            message: message,
                            isSystem: false,
                            files: [],
                          );
                          final newHistory = List<Message>.from(
                            state.messageHistory,
                          )..add(newMessage);
                          chatBloc.add(UpdateHistory(newHistory: newHistory));
                        },
                      ),
                    ],
                  ),
                ),
                _ => const Center(child: Text("Something went wrong.")),
              };
            },
          ),
        );
      },
    );
  }
}
