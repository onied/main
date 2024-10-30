import classes from "./chatWindow.module.css";
import ChatMessage from "@onied/components/supportChatForUser/chatMessage/chatMessage";
import {
  MessageDto,
  MessagesHistoryDto,
} from "@onied/components/supportChatForUser/messageDtos";
import ChatInput from "@onied/components/supportChatForUser/chatInput/chatInput";
import { useEffect, useRef } from "react";

type ChatWindowProps = {
  isChatWindowOpen: boolean;
  isFirstEverMessage: boolean;
  messagesHistory: MessagesHistoryDto;
  setMessagesHistory: (messagesHistory: MessagesHistoryDto) => void;
  sendMessage: (messageContent: string) => void;
  sendMessageDisabled: boolean;
};

function ChatWindow(props: ChatWindowProps) {
  if (!props.isChatWindowOpen) return <></>;
  const bottom = useRef<null | HTMLDivElement>(null);
  const scrollToBottom = () => {
    bottom.current?.scrollIntoView({ behavior: "smooth" });
  };
  useEffect(() => {
    scrollToBottom();
  }, [props.messagesHistory]);
  return (
    <div className={classes.chatWindow}>
      <div className={classes.chatHeader}>
        {props.isFirstEverMessage ? (
          <div>
            <p>Введите вопрос,</p>
            <p>чтобы начать диалог</p>
          </div>
        ) : props.messagesHistory.supportNumber === null ? (
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
                  index > 0 &&
                  array[index - 1].supportNumber === null &&
                  array[index].supportNumber !== null
                }
              ></ChatMessage>
            )
          )}
          <div ref={bottom}></div>
        </div>
      )}
      <ChatInput
        sendMessageDisabled={props.sendMessageDisabled}
        sendMessageToHub={props.sendMessage}
      ></ChatInput>
    </div>
  );
}

export default ChatWindow;
