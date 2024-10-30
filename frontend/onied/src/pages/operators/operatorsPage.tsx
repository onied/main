import { Side } from "@onied/types/general";
import OperatorsHeader from "./components/operatorsHeader/operatorsHeader";
import ChatsSidebar from "./components/ChatsSidebar/chatsSidebar";
import Background from "./components/background/background";
import ActiveArea from "./components/activeArea/activeArea";
import { useAppDispatch, useAppSelector } from "@onied/hooks";
import { useEffect } from "react";
import OperatorChatApi from "@onied/api/operatorChat";
import { ChatsStateActionTypes } from "@onied/redux/reducers/chatReducer";
import useSignalR from "@onied/hooks/signalr";
import Config from "@onied/config/config";
import { chatHubOperatorConnection } from "./chatHubOperatorConnection";
import { Message } from "@onied/types/chat";

export default function OperatorsPage() {
  const dispatch = useAppDispatch();
  const chatsState = useAppSelector((state) => state.chats);
  const { connection } = useSignalR(
    Config.BaseURL.replace(/\/$/, "") + "/chat/hub"
  );
  console.log(chatsState);

  useEffect(() => {
    if (!connection || !chatsState.currentChat) return;
    const chatClient = chatHubOperatorConnection(connection);
    let chat = { ...chatsState.currentChat };
    let lastMessage: Message | undefined = undefined;
    chat.messages.map((message) => {
      if (
        message.supportNumber !== chatsState.operatorProfile?.number &&
        message.readAt == null
      ) {
        chatClient.send.MarkMessageAsRead(message.messageId);
        message.readAt = new Date();
      }
      lastMessage = message;
      return message;
    });
    dispatch({
      type: ChatsStateActionTypes.FETCH_CURRENT_CHAT,
      payload: {
        chat: chat,
        chatId: chatsState.currentChatId,
      },
    });
    dispatch({
      type: ChatsStateActionTypes.FETCH_ACTIVE_CHATS,
      payload: chatsState.activeChats.map((chat) =>
        chat.chatId == chatsState.currentChatId
          ? { ...chat, lastMessage: lastMessage }
          : chat
      ),
    });
  }, [chatsState.currentChat]);

  useEffect(() => {
    if (!connection || !chatsState.operatorProfile) return;
    const chatOperator = chatHubOperatorConnection(connection);
    return chatOperator.on.ReceiveMessageFromChat((chatId, message) => {
      if (chatsState.currentChat && chatsState.currentChatId == chatId) {
        dispatch({
          type: ChatsStateActionTypes.FETCH_CURRENT_CHAT,
          payload: {
            chat: {
              ...chatsState.currentChat,
              messages: [...chatsState.currentChat.messages, message],
            },
            chatId: chatsState.currentChatId,
          },
        });
      }
      if (chatsState.activeChats.find((chat) => chat.chatId == chatId)) {
        dispatch({
          type: ChatsStateActionTypes.FETCH_ACTIVE_CHATS,
          payload: chatsState.activeChats.map((chat) =>
            chat.chatId == chatId
              ? {
                  ...chat,
                  lastMessage: message,
                }
              : chat
          ),
        });
      } else if (chatsState.openChats.find((chat) => chat.chatId == chatId)) {
        dispatch({
          type: ChatsStateActionTypes.FETCH_OPEN_CHATS,
          payload: chatsState.openChats.map((chat) =>
            chat.chatId == chatId ? { ...chat, lastMessage: message } : chat
          ),
        });
      } else {
        dispatch({
          type: ChatsStateActionTypes.FETCH_OPEN_CHATS,
          payload: [
            ...chatsState.openChats,
            { chatId: chatId, lastMessage: message },
          ],
        });
      }
    });
  }, [connection, chatsState.operatorProfile]);

  useEffect(() => {
    if (!connection) return;
    const chatOperator = chatHubOperatorConnection(connection);
    return chatOperator.on.ReceiveReadAt((messageId, readAt) => {
      chatsState.currentChat?.messages.find(
        (message) => message.messageId == messageId
      );
      if (chatsState.currentChat) {
        dispatch({
          type: ChatsStateActionTypes.FETCH_CURRENT_CHAT,
          payload: {
            chat: chatsState.currentChat.messages.map((message) =>
              message.messageId == messageId
                ? { ...message, readAt: readAt }
                : message
            ),
            chatId: chatsState.currentChatId,
          },
        });
      }
    });
  }, [connection]);

  useEffect(() => {
    if (!connection) return;
    const chatOperator = chatHubOperatorConnection(connection);
    return chatOperator.on.RemoveChatFromOpened((chatId) => {
      dispatch({
        type: ChatsStateActionTypes.FETCH_OPEN_CHATS,
        payload: chatsState.openChats.filter((chat) => chat.chatId != chatId),
      });
    });
  }, [connection]);

  return (
    <>
      <OperatorsHeader />
      <main>
        <Background />
        <ActiveChatsSidebar />
        {chatsState.currentChat && chatsState.currentChatId && (
          <ActiveArea
            activeChat={chatsState.currentChat}
            currentChatId={chatsState.currentChatId}
          />
        )}
        <OpenChatsSidebar />
      </main>
    </>
  );
}

const ActiveChatsSidebar = () => {
  const dispatch = useAppDispatch();
  const chatsState = useAppSelector((state) => state.chats);
  const badges = chatsState.activeChats;
  const opApi = new OperatorChatApi();
  useEffect(() => {
    opApi
      .GetActiveChats()
      .then((chats) => {
        dispatch({
          type: ChatsStateActionTypes.FETCH_ACTIVE_CHATS,
          payload: chats,
        });
      })
      .catch((error) => {
        console.error(error);
        dispatch({
          type: ChatsStateActionTypes.FETCH_ACTIVE_CHATS,
        });
      });
  }, []);
  return (
    <ChatsSidebar
      title={"Активные обращения"}
      badges={badges}
      side={Side.Left}
      searchEnabled={false}
    />
  );
};

const OpenChatsSidebar = () => {
  const dispatch = useAppDispatch();
  const chatsState = useAppSelector((state) => state.chats);
  const badges = chatsState.openChats;
  const opApi = new OperatorChatApi();
  useEffect(() => {
    opApi
      .GetOpenChats()
      .then((chats) => {
        dispatch({
          type: ChatsStateActionTypes.FETCH_OPEN_CHATS,
          payload: chats,
        });
      })
      .catch((error) => {
        console.error(error);
        dispatch({
          type: ChatsStateActionTypes.FETCH_OPEN_CHATS,
        });
      });
  }, []);
  return (
    <ChatsSidebar
      title={"Открытые обращения"}
      badges={badges}
      side={Side.Right}
      searchEnabled={true}
    />
  );
};
