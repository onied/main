import { useEffect, useState } from "react";
import ChatButton from "@onied/components/supportChatForUser/chatButton/chatButton";
import ChatWindow from "@onied/components/supportChatForUser/chatWindow/chatWindow";
import { MessagesHistoryDto } from "@onied/components/supportChatForUser/messageDtos";
import GetHistory from "@onied/components/supportChatForUser/tempMessagesSource";

function SupportChatWrapper() {
  const [isChatWindowOpen, setIsChatWindowOpen] = useState(false);

  const [messagesHistory, setMessagesHistory] = useState<MessagesHistoryDto>();

  useEffect(() => {
    setMessagesHistory(GetHistory(0));
  }, []);

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
        messagesHistory={messagesHistory!}
        setMessagesHistory={setMessagesHistory}
      ></ChatWindow>
      <ChatButton
        isChatWindowOpen={isChatWindowOpen}
        setIsChatWindowOpen={setIsChatWindowOpen}
      ></ChatButton>
    </div>
  );
}

export default SupportChatWrapper;
