import { UUID } from "crypto";

export type MessageFile = {
  filename: string;
  fileUrl: string;
};

export type Message = {
  messageId: UUID;
  supportNumber: number | null;
  createdAt: string;
  readAt: string | null;
  message: string;
  isSystem: boolean;
  files: MessageFile[];
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
