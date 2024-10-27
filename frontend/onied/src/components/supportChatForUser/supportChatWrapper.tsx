import { useState } from "react";
import ChatButton from "@onied/components/supportChatForUser/chatButton/chatButton";
import ChatWindow from "@onied/components/supportChatForUser/chatWindow/chatWindow";
import { MessagesHistoryDto } from "@onied/components/supportChatForUser/messageDtos";

function SupportChatWrapper() {
  const [isChatWindowOpen, setIsChatWindowOpen] = useState(false);

  const [messagesHistory, setMessagesHistory] =
    useState<MessagesHistoryDto | null>();

  setMessagesHistory({
    supportNumber: 69,
    currentSessionId: "6c27c121-8cf2-4db2-8c74-8834cb735fcd",
    messages: [
      {
        messageId: "87dbe1eb-b8e5-4c0c-abac-7a8784ed5ff0",
        supportNumber: null,
        createdAt: "imagine some date and time",
        messageText: "open-session 6c27c121-8cf2-4db2-8c74-8834cb735fcd",
        isSystem: true,
        readAt: "imagine some date and time",
      },
    ],
  });

  return (
    <div
      style={{
        display: "inline-block",
        position: "fixed",
        padding: "20px",
        right: 0,
        bottom: 0,
        zIndex: 9999,
      }}
    >
      <ChatWindow isChatWindowOpen={isChatWindowOpen}></ChatWindow>
      <ChatButton
        isChatWindowOpen={isChatWindowOpen}
        setIsChatWindowOpen={setIsChatWindowOpen}
      ></ChatButton>
    </div>
  );
}

export default SupportChatWrapper;
