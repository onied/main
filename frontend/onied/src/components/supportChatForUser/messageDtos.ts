import { UUID } from "crypto";

export type MessageDto = {
  messageId: UUID;
  supportNumber: number | null;
  createdAt: string;
  message: string;
  isSystem: boolean;
  readAt: string | null;
};

export type MessagesHistoryDto = {
  supportNumber: number | null;
  currentSessionId: UUID | null;
  messages: MessageDto[];
};
