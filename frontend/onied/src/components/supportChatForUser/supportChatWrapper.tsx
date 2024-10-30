import { useEffect, useRef, useState } from "react";
import api from "@onied/config/axios";
import ChatButton from "@onied/components/supportChatForUser/chatButton/chatButton";
import ChatWindow from "@onied/components/supportChatForUser/chatWindow/chatWindow";
import {
  MessageDto,
  MessagesHistoryDto,
} from "@onied/components/supportChatForUser/messageDtos";
import { AxiosError } from "axios";
import { useProfile } from "@onied/hooks/profile/useProfile";
import useSignalR from "@onied/hooks/signalr";
import Config from "@onied/config/config";
import { chatHubClientConnection } from "./chatHubClientConnection";

function SupportChatWrapper() {
  const [profile, _] = useProfile();
  const [isChatWindowOpen, setIsChatWindowOpen] = useState(false);
  const [isFirstEverMessage, setIsFirstEverMessage] = useState(false);
  const unreadCount = useRef(0);
  const [unreadCountReactive, setUnreadCount] = useState(unreadCount.current);
  const [messagesHistory, setMessagesHistory] = useState<MessagesHistoryDto>();
  const { connection } = useSignalR(
    Config.BaseURL.replace(/\/$/, "") + "/chat/hub"
  );

  const fetchChat = () => {
    api
      .get("/chat")
      .then((response) => {
        let data = response.data;
        data.messages = data.messages.map((message: MessageDto) => {
          return { ...message, createdAt: new Date(message.createdAt) };
        });
        setMessagesHistory(data);
        setUnreadCount(unreadCount.current);
      })
      .catch((error: AxiosError<any, any>) => {
        if (error.response != null) {
          if (error.response.status == 404) {
            setIsFirstEverMessage(true);
          }
        } else {
          console.error("Could not reach the server.");
        }
      });
  };

  const sendMessage = (messageContent: string) => {
    if (!connection) return;
    const chatClient = chatHubClientConnection(connection);
    chatClient.send.SendMessage(messageContent);
  };

  useEffect(fetchChat, []);

  useEffect(() => {
    if (!connection) return;
    const chatClient = chatHubClientConnection(connection);
    if (!messagesHistory) return;
    let messageHistory = { ...messagesHistory };
    unreadCount.current = 0;
    messageHistory.messages.map((message) => {
      if (message.supportNumber && message.readAt == null) {
        if (isChatWindowOpen) {
          chatClient.send.MarkMessageAsRead(message.messageId);
          message.readAt = new Date().toISOString();
        } else {
          unreadCount.current++;
        }
      }
      return message;
    });
    setMessagesHistory(messageHistory);
    setUnreadCount(unreadCount.current);
  }, [connection, isChatWindowOpen, messagesHistory]);

  useEffect(() => {
    if (!connection) return;
    const chatClient = chatHubClientConnection(connection);
    return chatClient.on.ReceiveMessage((message) => {
      if (messagesHistory == undefined) return;
      let messageHistory = { ...messagesHistory };
      messageHistory.messages.push(message);
      if (
        message.supportNumber != null &&
        messageHistory.supportNumber != message.supportNumber
      ) {
        messageHistory.supportNumber = message.supportNumber;
        fetchChat();
      }
      setMessagesHistory(messageHistory);
    });
  }, [connection]);

  useEffect(() => {
    if (!connection) return;
    const chatClient = chatHubClientConnection(connection);
    return chatClient.on.ReceiveReadAt((messageId, readAt) => {
      if (messagesHistory == undefined) return;
      let messageHistory = { ...messagesHistory };
      messageHistory.messages.map((message) =>
        message.messageId == messageId
          ? { ...message, readAt: readAt }
          : message
      );
      setMessagesHistory(messageHistory);
    });
  }, [connection]);

  if (!profile || !messagesHistory) return <></>;

  return (
    <div
      style={{
        display: "inline-block",
        position: "fixed",
        maxWidth: "22vw",
        minWidth: "20vw",
        padding: "20px",
        right: 0,
        bottom: 0,
        zIndex: 9999,
      }}
    >
      <ChatWindow
        isChatWindowOpen={isChatWindowOpen}
        isFirstEverMessage={isFirstEverMessage}
        messagesHistory={
          messagesHistory
            ? messagesHistory
            : ({
                supportNumber: null,
                currentSessionId: null,
                messages: [],
              } as MessagesHistoryDto)
        }
        setMessagesHistory={setMessagesHistory}
        sendMessageDisabled={connection === undefined}
        sendMessage={sendMessage}
      ></ChatWindow>
      <ChatButton
        isChatWindowOpen={isChatWindowOpen}
        setIsChatWindowOpen={setIsChatWindowOpen}
        unreadCount={unreadCountReactive}
      ></ChatButton>
    </div>
  );
}

export default SupportChatWrapper;
