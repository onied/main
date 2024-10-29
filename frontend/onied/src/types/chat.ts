import { UUID } from "crypto";

export type Message = {
  MessageId: UUID;
  SupportNumber: number | null;
  CreatedAt: number;
  ReadAt: number | null;
  Message: string;
  IsSystem: boolean;
};

export type Chat = {
  SupportNumber: number | null;
  CurrentSessionId: UUID | null;
  Messages: Message[];
};

export type ChatBadge = {
  ChatId: UUID;
  LastMessage: Message;
};

export type OperatorProfile = {
  Number: number;
};
