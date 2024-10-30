import { UUID } from "crypto";

export type MessageDto = {
  messageId: UUID;
  supportNumber: number | null;
  createdAt: Date;
  message: string;
  isSystem: boolean;
  readAt: Date | null;
};

export type MessagesHistoryDto = {
  supportNumber: number | null;
  currentSessionId: UUID | null;
  messages: MessageDto[];
};
