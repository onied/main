import classes from "./chatWindow.module.css";
import ChatMessage from "@onied/components/supportChatForUser/chatMessage/chatMessage";
import {
  MessageDto,
  MessagesHistoryDto,
} from "@onied/components/supportChatForUser/messageDtos";
import ChatInput from "@onied/components/supportChatForUser/chatInput/chatInput";
import { useEffect } from "react";

type ChatWindowProps = {
  isChatWindowOpen: boolean;
  messagesHistory: MessagesHistoryDto;
  setMessagesHistory: (messagesHistory: MessagesHistoryDto) => void;
};

function ChatWindow(props: ChatWindowProps) {
  if (!props.isChatWindowOpen) return <></>;
  return (
    <div className={classes.chatWindow}>
      <div className={classes.chatHeader}>
        {props.messagesHistory === null ||
        props.messagesHistory.supportNumber === null ? (
          <div>
            <p>Поиск оператора.</p>
            <p>Пожалуйста подождите...</p>
          </div>
        ) : (
          <div>
            <p>На ваш вопрос ответит</p>
            <p>Оператор №{props.messagesHistory.supportNumber}</p>
          </div>
        )}
      </div>
      {props.messagesHistory === null ? (
        <></>
      ) : (
        <div className={classes.messagesTimeline}>
          {props.messagesHistory.messages.map(
            (message: MessageDto, index: number, array: MessageDto[]) => (
              <ChatMessage
                key={message.messageId}
                {...message}
                isFirstOperatorMessageInChain={
                  !message.isSystem &&
                  array[index - 1].supportNumber === null &&
                  array[index].supportNumber !== null
                }
              ></ChatMessage>
            )
          )}
        </div>
      )}
      <ChatInput></ChatInput>
    </div>
  );
}

export default ChatWindow;
