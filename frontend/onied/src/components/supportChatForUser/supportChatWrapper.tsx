import { useState } from "react";
import ChatButton from "@onied/components/supportChatForUser/chatButton/chatButton";
import ChatWindow from "@onied/components/supportChatForUser/chatWindow/chatWindow";

function SupportChatWrapper() {
  const [isChatWindowOpen, setIsChatWindowOpen] = useState(false);

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
