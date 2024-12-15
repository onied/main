import { HubConnection } from "@microsoft/signalr";
import { UUID } from "crypto";
import { FileDto, MessageDto } from "./messageDtos";

export function chatHubClientConnection(connection: HubConnection) {
  const onMethod = (
    method: string,
    actionsOnReceive: (...args: any[]) => any
  ) => {
    connection.on(method, actionsOnReceive);
    return () => connection.off(method);
  };
  return {
    on: {
      ReceiveMessage: (actionsOnReceive: (message: MessageDto) => void) =>
        onMethod("ReceiveMessage", actionsOnReceive),
      ReceiveReadAt: (
        actionsOnReceive: (messageId: UUID, readAt: string) => void
      ) => onMethod("ReceiveReadAt", actionsOnReceive),
    },
    send: {
      SendMessage: (messageContent: string, files: FileDto[]) => {
        connection.send("SendMessage", messageContent, files);
      },
      MarkMessageAsRead: (messageId: UUID) => {
        connection.send("MarkMessageAsRead", messageId);
      },
    },
  };
}
