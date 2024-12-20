import { HubConnection } from "@microsoft/signalr";
import { UUID } from "crypto";
import { MessageFile, Message } from "@onied/types/chat";

export function chatHubOperatorConnection(connection: HubConnection) {
  const onMethod = (
    method: string,
    actionsOnReceive: (...args: any[]) => any
  ) => {
    connection.on(method, actionsOnReceive);
    console.log("Registered method " + method);
    return () => connection.off(method);
  };
  return {
    on: {
      ReceiveReadAt: (
        actionsOnReceive: (messageId: UUID, readAt: string) => void
      ) => onMethod("ReceiveReadAt", actionsOnReceive),
      ReceiveMessageFromChat: (
        actionsOnReceive: (chatId: UUID, message: Message) => void
      ) => onMethod("ReceiveMessageFromChat", actionsOnReceive),
      RemoveChatFromOpened: (actionsOnReceive: (chatId: UUID) => void) =>
        onMethod("RemoveChatFromOpened", actionsOnReceive),
    },
    send: {
      MarkMessageAsRead: (messageId: UUID) => {
        connection.send("MarkMessageAsRead", messageId);
      },
      SendMessageToChat: (
        chatId: UUID,
        messageContent: string,
        files: MessageFile[]
      ) => {
        connection.send("SendMessageToChat", chatId, messageContent, files);
      },
      CloseChat: (chatId: UUID) => {
        connection.send("CloseChat", chatId);
      },
      AbandonChat: (chatId: UUID) => {
        connection.send("AbandonChat", chatId);
      },
    },
  };
}
