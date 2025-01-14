import { UUID } from "crypto";

export type FileDto = {
  filename: string;
  fileUrl: string;
};

export type MessageDto = {
  messageId: UUID;
  supportNumber: number | null;
  createdAt: string;
  message: string;
  isSystem: boolean;
  readAt: string | null;
  files: FileDto[];
};

export type MessagesHistoryDto = {
  supportNumber: number | null;
  currentSessionId: UUID | null;
  messages: MessageDto[];
};
