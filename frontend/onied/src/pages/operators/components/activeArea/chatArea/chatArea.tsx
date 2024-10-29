import classes from "./chatArea.module.css";

import combineCssClasses from "@onied/helpers/combineCssClasses";
import SendMessageFooter from "../sendMessageFooter/sendMessageFooter";
import { Chat, Message } from "@onied/types/chat";

const ReadBadge = () => {
  return (
    <svg
      width="20"
      height="20"
      viewBox="0 0 20 20"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <g clipPath="url(#clip0_1668_382)">
        <path
          d="M19.7071 3.44904C19.3166 3.05849 18.6835 3.05849 18.2929 3.44904L6.31228 
                    15.4298L1.70713 10.8246C1.31662 10.4341 0.683496 10.4341 0.29291 10.8246C-0.0976367 
                    11.2151 -0.0976367 11.8483 0.29291 12.2388L5.60518 17.551C5.99557 17.9415 6.62916 
                    17.9412 7.01939 17.551L19.7071 4.86326C20.0977 4.47275 20.0976 3.83958 19.7071 3.44904Z"
          fill="#00D9FF"
        />
      </g>
      <defs>
        <clipPath id="clip0_1668_382">
          <rect width="20" height="20" fill="white" />
        </clipPath>
      </defs>
    </svg>
  );
};

const TimeBadge = ({ time }: { time: number }) => {
  const getShowTime = (unixTime: number) => {
    const date = new Date(unixTime * 1000);
    const hours = date.getHours().toString();
    const minutes = ("0" + date.getMinutes()).substr(-2);
    return `${hours}:${minutes}`;
  };

  return <span className={classes.timeBadge}>{getShowTime(time)}</span>;
};

const ChatMessage = ({ message }: { message: Message }) => {
  const isMine = message.supportNumber != null;

  return (
    <div
      className={combineCssClasses([
        classes.chatMessage,
        isMine ? classes.mine : classes.other,
      ])}
    >
      <p>{message.message}</p>
      <div className={classes.chatFooter}>
        {message.readAt && <ReadBadge />}
        <TimeBadge time={message.createdAt} />
      </div>
    </div>
  );
};

export default function ChatArea({ chat }: { chat: Chat }) {
  return (
    <div className={classes.chatArea}>
      {chat.messages.map((msg) => (
        <ChatMessage message={msg} />
      ))}
      <SendMessageFooter />
    </div>
  );
}
