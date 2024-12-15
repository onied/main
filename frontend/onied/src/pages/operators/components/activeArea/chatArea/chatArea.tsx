import Config from "@onied/config/config";
import classes from "./chatArea.module.css";

import combineCssClasses from "@onied/helpers/combineCssClasses";
import { Chat, Message } from "@onied/types/chat";
import { useEffect, useRef } from "react";

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

const TimeBadge = ({ time }: { time: Date }) => {
  return (
    <span className={classes.timeBadge}>
      {time.toLocaleString([], {
        minute: "numeric",
        hour: "numeric",
        day: "numeric",
        month: "numeric",
        year: "numeric",
      })}
    </span>
  );
};

function SystemMessage({ message }: { message: string }) {
  const parsedMessage = message.startsWith("open-session ")
    ? `Начало сессии ${message.split(" ")[1]}`
    : message === "close-session"
      ? "Конец сессии"
      : undefined;
  if (!parsedMessage) return <></>;
  return (
    <div className={classes.systemMessage}>
      <div className={classes.line}></div>
      <span>{parsedMessage}</span>
      <div className={classes.line}></div>
    </div>
  );
}

const ChatMessage = ({ message }: { message: Message }) => {
  const isMine = message.supportNumber != null;

  if (message.isSystem) return <SystemMessage message={message.message} />;
  return (
    <div
      className={combineCssClasses([
        classes.chatMessage,
        isMine ? classes.mine : classes.other,
      ])}
    >
      <p>{message.message}</p>
      {message.files.length > 0 && (
        <ul className={classes.chatFiles}>
          {message.files.map((file) => {
            return (
              <li key={file.fileUrl}>
                <a
                  href={
                    Config.BaseURL.replace(/\/$/, "") +
                    "/get/storage/" +
                    file.fileUrl.replace(/^\//, "")
                  }
                  className={classes.chatFile}
                  target="_blank"
                >
                  {file.filename}
                </a>
              </li>
            );
          })}
        </ul>
      )}
      <div className={classes.chatFooter}>
        {message.readAt && <ReadBadge />}
        <TimeBadge time={new Date(message.createdAt)} />
      </div>
    </div>
  );
};

export default function ChatArea({ chat }: { chat: Chat }) {
  const bottom = useRef<null | HTMLDivElement>(null);
  const scrollToBottom = () => {
    bottom.current?.scrollIntoView({ behavior: "smooth" });
  };
  useEffect(() => {
    scrollToBottom();
  }, [chat]);
  return (
    <div className={classes.chatAreaWrapperWrapper}>
      <div className={classes.chatAreaWrapper}>
        <div className={classes.chatArea}>
          {chat.messages.map((msg) => (
            <ChatMessage message={msg} key={msg.messageId} />
          ))}
          <div ref={bottom}></div>
        </div>
      </div>
    </div>
  );
}
