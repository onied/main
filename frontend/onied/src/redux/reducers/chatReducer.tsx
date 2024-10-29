import { Chat, ChatBadge, OperatorProfile } from "@onied/types/chat";

type ChatsState = {
  operatorProfile?: OperatorProfile;
  activeChats: ChatBadge[];
  openChats: ChatBadge[];
  currentChat?: Chat;
};

const initialState: ChatsState = {
  operatorProfile: { number: 14 },
  activeChats: [
    {
      chatId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
      lastMessage: {
        messageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        supportNumber: 69,
        createdAt: 0,
        readAt: null,
        isSystem: false,
        message: "А что кушают поросятки?",
      },
    },
    {
      chatId: "62cbfd28-0c25-4898-9a2e-dae00719586d",
      lastMessage: {
        messageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        supportNumber: 69,
        createdAt: 0,
        readAt: null,
        isSystem: false,
        message: "А что кушают поросятки?",
      },
    },
    {
      chatId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
      lastMessage: {
        messageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        supportNumber: 69,
        createdAt: 0,
        readAt: null,
        isSystem: false,
        message: "бла-бла-бла",
      },
    },
  ],
  openChats: [
    {
      chatId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
      lastMessage: {
        messageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        supportNumber: 69,
        createdAt: 0,
        readAt: null,
        isSystem: false,
        message: "А что кушают поросятки?",
      },
    },
    {
      chatId: "62cbfd28-0c25-4898-9a2e-dae00719586d",
      lastMessage: {
        messageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        supportNumber: 69,
        createdAt: 0,
        readAt: null,
        isSystem: false,
        message: "А что кушают поросятки?",
      },
    },
    {
      chatId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
      lastMessage: {
        messageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        supportNumber: 69,
        createdAt: 0,
        readAt: null,
        isSystem: false,
        message: "бла-бла-бла",
      },
    },
  ],
  currentChat: {
    supportNumber: 69,
    currentSessionId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
    messages: [
      {
        messageId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
        supportNumber: null,
        createdAt: 1730115891,
        readAt: 1730115891,
        isSystem: false,
        message:
          "бла-бла-бла esfesfdsfds esfesfdsfds esfesfdsfds asda dsadsa da sadas",
      },
      {
        messageId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
        supportNumber: 69,
        createdAt: 1730115891,
        readAt: 1730115891,
        isSystem: false,
        message:
          "бла-бла-бла esfesfdsfds esfesfdsfds esfesfdsfds asda dsadsa da sadas",
      },
    ],
  },
};

export enum ChatsStateActionTypes {
  FETCH_OPERATOR_PROFILE = "FETCH_OPERATOR_PROFILE",
  FETCH_ACTIVE_CHATS = "FETCH_ACTIVE_CHATS",
  FETCH_OPEN_CHATS = "FETCH_OPEN_CHATS",
  FETCH_CURRENT_CHAT = "FETCH_CURRENT_CHAT",
}
interface FetchOperatorProfileAction {
  type: ChatsStateActionTypes.FETCH_OPERATOR_PROFILE;
  payload: OperatorProfile | undefined;
}
interface FetchActiveChatsAction {
  type: ChatsStateActionTypes.FETCH_ACTIVE_CHATS;
  payload: ChatBadge[] | undefined;
}
interface FetchOpenChatsAction {
  type: ChatsStateActionTypes.FETCH_OPEN_CHATS;
  payload: ChatBadge[] | undefined;
}
interface FetchCurrentChatAction {
  type: ChatsStateActionTypes.FETCH_CURRENT_CHAT;
  payload: Chat | undefined;
}

export type ChatsAction =
  | FetchOperatorProfileAction
  | FetchActiveChatsAction
  | FetchOpenChatsAction
  | FetchCurrentChatAction;

export const ChatsReducer = (state = initialState, action: ChatsAction) => {
  console.log(action);
  switch (action.type) {
    case ChatsStateActionTypes.FETCH_OPERATOR_PROFILE:
      return {
        ...state,
        operatorProfile: action.payload,
      };
    case ChatsStateActionTypes.FETCH_ACTIVE_CHATS:
      return {
        ...state,
        activeChats: action.payload ?? [],
      };
    case ChatsStateActionTypes.FETCH_OPEN_CHATS:
      return {
        ...state,
        openChats: action.payload ?? [],
      };
    case ChatsStateActionTypes.FETCH_CURRENT_CHAT:
      return {
        ...state,
        currentChat: action.payload,
      };
    default:
      return state;
  }
};
