import 'package:flutter/material.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/models/chat/message.dart';

class MessageContainer extends StatelessWidget {
  final Message message;

  const MessageContainer({super.key, required this.message});

  @override
  Widget build(BuildContext context) {
    final dateTime = message.createdAt;
    final dateTimeString =
        "${dateTime.hour}:${dateTime.minute < 10 ? "0${dateTime.minute}" : dateTime.minute.toString()}";

    return Row(
      mainAxisAlignment:
          message.supportNumber == 0
              ? MainAxisAlignment.end
              : MainAxisAlignment.start,
      children: [
        Flexible(
          child: Container(
            constraints: BoxConstraints(
              maxWidth: MediaQuery.of(context).size.width * 0.7,
            ),
            decoration: BoxDecoration(
              color:
                  message.supportNumber == 0
                      ? AppTheme.accentBright
                      : AppTheme.otherMessageBackground,
              borderRadius: BorderRadius.only(
                topLeft: const Radius.circular(10),
                topRight: const Radius.circular(10),
                bottomRight: Radius.circular(
                  message.supportNumber == 0 ? 0 : 10,
                ),
                bottomLeft: Radius.circular(
                  message.supportNumber == 0 ? 10 : 0,
                ),
              ),
            ),
            padding: const EdgeInsets.all(16),
            child: IntrinsicWidth(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.end,
                children: [
                  Text(
                    message.message,
                    textAlign: TextAlign.start,
                    style: TextStyle(
                      color:
                          message.supportNumber == 0
                              ? Colors.white
                              : Colors.black,
                    ),
                  ),
                  const SizedBox(height: 8),
                  Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Text(
                        dateTimeString,
                        style: TextStyle(
                          color: AppTheme.timeMessageColor,
                          fontSize: 14,
                        ),
                      ),
                      if (message.readAt != null) ...[
                        const SizedBox(width: 8),
                        Icon(
                          Icons.mark_chat_read_outlined,
                          size: 14,
                          color: AppTheme.markMessageAsRead,
                        ),
                      ],
                    ],
                  ),
                ],
              ),
            ),
          ),
        ),
      ],
    );
  }
}
