import classes from "./chatWindow.module.css";

type ChatWindowProps = {
  isChatWindowOpen: boolean;
};

function ChatWindow(props: ChatWindowProps) {
  if (!props.isChatWindowOpen) return <></>;
  return (
    <div>
      <div className={classes.chatHeader}>
        <p>Поиск оператора.</p>
        <p>Пожалуйста подождите...</p>
      </div>
    </div>
  );
}

export default ChatWindow;
