import classes from "./chatButton.module.css";
import supportIcon from "../../../assets/supportIcon.svg";
import cross from "../../../assets/whiteCross.svg";
import Button from "@onied/components/general/button/button";

type ChatButtonProps = {
  isChatWindowOpen: boolean;
  setIsChatWindowOpen: (isChatWindowOpen: boolean) => void;
};

function ChatButton(props: ChatButtonProps) {
  return (
    <div className={classes.buttonWrapper}>
      <Button
        style={{ borderRadius: "50%", padding: "20px", margin: 0 }}
        onClick={() => props.setIsChatWindowOpen(!props.isChatWindowOpen)}
      >
        <img src={props.isChatWindowOpen ? cross : supportIcon} />
      </Button>
    </div>
  );
}

export default ChatButton;
