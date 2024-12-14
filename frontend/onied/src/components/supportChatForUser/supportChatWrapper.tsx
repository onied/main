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
  const [isFirstMessageInSession, setIsFirstMessageInSession] = useState(false);
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
            setIsFirstMessageInSession(true);
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
    setIsFirstMessageInSession(false);
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
            console.log(message);
            message.readAt = new Date().toISOString();
          } else {
            unreadCount.current++;
          }
        }
        return message;
      }),
    });
    setUnreadCount(unreadCount.current);
    const len = messagesHistory.messages.length;
    if (len > 0) {
      const last = messagesHistory.messages[len - 1];
      if (last.isSystem && last.message == "close-session")
        setIsFirstMessageInSession(true);
    }
  }, [connection, isChatWindowOpen, messagesHistory?.messages.length]);

  useEffect(() => {
    if (!connection) return;
    const chatClient = chatHubClientConnection(connection);
    return chatClient.on.ReceiveMessage((message) => {
      console.log("Receive message", message, messagesHistory);
      if (messagesHistory == undefined) {
        setMessagesHistory({
          messages: [{ ...message, readAt: null }],
          supportNumber: message.supportNumber,
          currentSessionId: null,
        });
      } else {
        setMessagesHistory({
          ...messagesHistory,
          messages: [...messagesHistory.messages, { ...message, readAt: null }],
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
      }
    });
  }, [connection, messagesHistory, isFirstMessageInSession]);

  useEffect(() => {
    if (!connection) return;
    const chatClient = chatHubClientConnection(connection);
    return chatClient.on.ReceiveReadAt((messageId, readAt) => {
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
  }, [connection, messagesHistory]);

  if (!profile || (!messagesHistory && !isFirstMessageInSession)) return <></>;

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
        zIndex: 1000,
      }}
    >
      <ChatWindow
        isChatWindowOpen={isChatWindowOpen}
        isFirstMessageInSession={isFirstMessageInSession}
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
