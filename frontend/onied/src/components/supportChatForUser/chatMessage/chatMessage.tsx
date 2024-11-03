import classes from "./chatMessage.module.css";
import tick from "../../../assets/cyanTick.svg";
import anon from "../../../assets/anonOperatorIcon.png";
import { MessageDto } from "@onied/components/supportChatForUser/messageDtos";

type ChatMessageProps = MessageDto & { isFirstOperatorMessageInChain: boolean };

function ChatMessage(props: ChatMessageProps) {
  if (props.isSystem) {
    switch (props.message.split(" ")[0]) {
      case "close-session":
        return <DialogEndSystemMessage></DialogEndSystemMessage>;
      default:
        return <></>;
    }
  }

  return (
    <PersonsMessage
      {...props}
      isFirstOperatorMessageInChain={props.isFirstOperatorMessageInChain}
    ></PersonsMessage>
  );
}

function PersonsMessage(props: ChatMessageProps) {
  return (
    <>
      {props.isFirstOperatorMessageInChain ? (
        <div className={classes.newOperatorMessageHeader}>
          <img src={anon} />
          <span>Оператор №{props.supportNumber}</span>
        </div>
      ) : (
        <></>
      )}

      <div
        className={[
          classes.chatMessage,
          props.supportNumber === null
            ? classes.ownMessage
            : classes.foreignMessage,
        ].join(" ")}
      >
        <p>{props.message}</p>
        <div className={classes.chatMessageFooter}>
          {props.readAt !== null && props.supportNumber === null ? (
            <img src={tick} />
          ) : (
            <></>
          )}
          <p>
            {new Date(props.createdAt).toLocaleString([], {
              minute: "numeric",
              hour: "numeric",
              day: "numeric",
              month: "numeric",
              year: "numeric",
            })}
          </p>
        </div>
      </div>
    </>
  );
}

function DialogEndSystemMessage() {
  return (
    <div className={classes.systemMessage}>
      <div className={classes.line}></div>
      <span>Диалог завершен</span>
      <div className={classes.line}></div>
    </div>
  );
}

export default ChatMessage;
