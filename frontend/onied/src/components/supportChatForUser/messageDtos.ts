export type MessageDto = {
  messageId: string;
  supportNumber: number | null;
  createdAt: Date;
  message: string;
  isSystem: boolean;
  readAt: Date | null;
};

export type MessagesHistoryDto = {
  supportNumber: number | null;
  currentSessionId: string | null;
  messages: MessageDto[];
};
