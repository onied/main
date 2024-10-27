export type MessageDto = {
  messageId: string;
  supportNumber: number | null;
  createdAt: Date;
  messageText: string;
  isSystem: boolean;
  readAt: Date | null;
};

export type MessagesHistoryDto = {
  supportNumber: number;
  currentSessionId: string;
  messages: MessageDto[];
};
