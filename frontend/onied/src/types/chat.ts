import { UUID } from "crypto";

export type Message = {
  messageId: UUID;
  supportNumber: number | null;
  createdAt: string;
  readAt: string | null;
  message: string;
  isSystem: boolean;
};

export type Chat = {
  supportNumber: number | null;
  currentSessionId: UUID | null;
  messages: Message[];
};

export type ChatBadge = {
  chatId: UUID;
  lastMessage: Message;
};

export type OperatorProfile = {
  number: number;
};
