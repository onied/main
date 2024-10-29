import { Chat, ChatBadge, OperatorProfile } from "@onied/types/chat";

type ChatsState = {
  operatorProfile?: OperatorProfile;
  activeChats: ChatBadge[];
  openChats: ChatBadge[];
  currentChat?: Chat;
};

const initialState: ChatsState = {
  operatorProfile: { Number: 14 },
  activeChats: [
    {
      ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
      LastMessage: {
        MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        SupportNumber: 69,
        CreatedAt: 0,
        ReadAt: null,
        IsSystem: false,
        Message: "А что кушают поросятки?",
      },
    },
    {
      ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586d",
      LastMessage: {
        MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        SupportNumber: 69,
        CreatedAt: 0,
        ReadAt: null,
        IsSystem: false,
        Message: "А что кушают поросятки?",
      },
    },
    {
      ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
      LastMessage: {
        MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        SupportNumber: 69,
        CreatedAt: 0,
        ReadAt: null,
        IsSystem: false,
        Message: "бла-бла-бла",
      },
    },
  ],
  openChats: [
    {
      ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
      LastMessage: {
        MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        SupportNumber: 69,
        CreatedAt: 0,
        ReadAt: null,
        IsSystem: false,
        Message: "А что кушают поросятки?",
      },
    },
    {
      ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586d",
      LastMessage: {
        MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        SupportNumber: 69,
        CreatedAt: 0,
        ReadAt: null,
        IsSystem: false,
        Message: "А что кушают поросятки?",
      },
    },
    {
      ChatId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
      LastMessage: {
        MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586c",
        SupportNumber: 69,
        CreatedAt: 0,
        ReadAt: null,
        IsSystem: false,
        Message: "бла-бла-бла",
      },
    },
  ],
  currentChat: {
    SupportNumber: 69,
    CurrentSessionId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
    Messages: [
      {
        MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
        SupportNumber: null,
        CreatedAt: 1730115891,
        ReadAt: 1730115891,
        IsSystem: false,
        Message:
          "бла-бла-бла esfesfdsfds esfesfdsfds esfesfdsfds asda dsadsa da sadas",
      },
      {
        MessageId: "62cbfd28-0c25-4898-9a2e-dae00719586e",
        SupportNumber: 69,
        CreatedAt: 1730115891,
        ReadAt: 1730115891,
        IsSystem: false,
        Message:
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
