import classes from "./chatMessage.module.css";
import tick from "../../../assets/cyanTick.svg";

type ChatMessageProps = {
  message: string;
  time: Date;
  isOwn: boolean;
  isRead: boolean;
};

function ChatMessage(props: ChatMessageProps) {
  return (
    <div
      className={[
        classes.chatMessage,
        props.isOwn ? classes.ownMessage : classes.foreignMessage,
      ].join(" ")}
    >
      <p>{props.message}</p>
      <div className={classes.chatMessageFooter}>
        <p>{props.time.toTimeString()}</p>
        {props.isRead ? <img src={tick} /> : <></>}
      </div>
    </div>
  );
}

export default ChatMessage;
