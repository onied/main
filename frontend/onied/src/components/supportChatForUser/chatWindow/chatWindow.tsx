import classes from "./chatWindow.module.css";
import ChatMessage from "@onied/components/supportChatForUser/chatMessage/chatMessage";
import { MessageDto } from "@onied/components/supportChatForUser/messageDtos";

type ChatWindowProps = {
  isChatWindowOpen: boolean;
  messagesList: MessageDto[];
};

function ChatWindow(props: ChatWindowProps) {
  if (!props.isChatWindowOpen) return <></>;
  return (
    <div className={classes.chatWindow}>
      <div className={classes.chatHeader}>
        <p>Поиск оператора.</p>
        <p>Пожалуйста подождите...</p>
      </div>
      <div className={classes.messagesTimeline}>
        {props.messagesList.map((message: MessageDto) => (
          <ChatMessage
            key={message.messageId}
            message={message.messageText}
            time={message.createdAt}
            isOwn={message.supportNumber === null}
            isRead={message.readAt !== null}
          ></ChatMessage>
        ))}
      </div>
    </div>
  );
}

export default ChatWindow;
