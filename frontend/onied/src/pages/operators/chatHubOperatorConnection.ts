import { HubConnection } from "@microsoft/signalr";
import { UUID } from "crypto";
import { Message } from "@onied/types/chat";

export function chatHubOperatorConnection(connection: HubConnection) {
  const onMethod = (
    method: string,
    actionsOnReceive: (...args: any[]) => any
  ) => {
    connection.on(method, actionsOnReceive);
    return () => connection.off(method);
  };
  return {
    on: {
      ReceiveReadAt: (
        actionsOnReceive: (messageId: UUID, readAt: Date) => void
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
      SendMessageToChat: (chatId: UUID, messageContent: string) => {
        connection.send("SendMessageToChat", chatId, messageContent);
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
