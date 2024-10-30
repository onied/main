import { useEffect, useRef, useState } from "react";
import api from "@onied/config/axios";
import ChatButton from "@onied/components/supportChatForUser/chatButton/chatButton";
import ChatWindow from "@onied/components/supportChatForUser/chatWindow/chatWindow";
import { MessagesHistoryDto } from "@onied/components/supportChatForUser/messageDtos";
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
        setMessagesHistory(response.data);
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
    unreadCount.current = 0;
    setMessagesHistory({
      ...messagesHistory,
      messages: messagesHistory.messages.map((message) => {
        if (message.supportNumber && message.readAt == null) {
          if (isChatWindowOpen) {
            chatClient.send.MarkMessageAsRead(message.messageId);
            message.readAt = new Date().toISOString();
          } else {
            unreadCount.current++;
          }
        }
        return message;
      }),
    });
    setUnreadCount(unreadCount.current);
  }, [connection, isChatWindowOpen, messagesHistory?.messages.length]);

  useEffect(() => {
    if (!connection) return;
    const chatClient = chatHubClientConnection(connection);
    return chatClient.on.ReceiveMessage((message) => {
      if (messagesHistory == undefined) return;
      setMessagesHistory({
        ...messagesHistory,
        messages: [...messagesHistory.messages, message],
        supportNumber: (() => {
          if (
            message.supportNumber &&
            messagesHistory.supportNumber != message.supportNumber
          ) {
            fetchChat();
            return message.supportNumber;
          }
          return messagesHistory.supportNumber;
        })(),
      });
    });
  }, [connection]);

  useEffect(() => {
    if (!connection) return;
    const chatClient = chatHubClientConnection(connection);
    return chatClient.on.ReceiveReadAt((messageId, readAt) => {
      console.log("READ AT HERE " + messageId + " " + readAt);
      console.log(messagesHistory);
      if (messagesHistory == undefined) return;
      setMessagesHistory({
        ...messagesHistory,
        messages: messagesHistory.messages.map((message) =>
          message.messageId == messageId
            ? { ...message, readAt: readAt }
            : message
        ),
      });
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
