import classes from "./activeArea.module.css";

import UpperBar from "./upperBar/upperBar";
import ChatArea from "./chatArea/chatArea";
import { Chat } from "@onied/types/chat";
import { UUID } from "crypto";

export default function ActiveArea({
  activeChat,
  currentChatId,
}: {
  activeChat: Chat;
  currentChatId: UUID;
}) {
  return (
    <div className={classes.activeArea}>
      <UpperBar currentChatId={currentChatId} />
      <ChatArea chat={activeChat} />
    </div>
  );
}
