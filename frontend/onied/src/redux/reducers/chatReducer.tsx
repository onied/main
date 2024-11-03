import { Chat, ChatBadge, OperatorProfile } from "@onied/types/chat";
import { UUID } from "crypto";

type ChatsState = {
  operatorProfile?: OperatorProfile;
  activeChats: ChatBadge[];
  openChats: ChatBadge[];
  currentChat?: Chat;
  currentChatId?: UUID;
};

const initialState: ChatsState = {
  operatorProfile: undefined,
  activeChats: [],
  openChats: [],
  currentChat: undefined,
  currentChatId: undefined,
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
  payload: { chat: Chat; chatId: UUID } | undefined;
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
        currentChat: action.payload?.chat,
        currentChatId: action.payload?.chatId,
      };
    default:
      return state;
  }
};
