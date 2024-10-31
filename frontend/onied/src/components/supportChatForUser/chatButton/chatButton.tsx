import classes from "./chatButton.module.css";
import supportIcon from "../../../assets/supportIcon.svg";
import cross from "../../../assets/whiteCross.svg";
import Button from "@onied/components/general/button/button";
import { debounce } from "@mui/material";
import { useState } from "react";

type ChatButtonProps = {
  isChatWindowOpen: boolean;
  setIsChatWindowOpen: (isChatWindowOpen: boolean) => void;
};

function ChatButton(props: ChatButtonProps) {
  const [isHovered, setIsHovered] = useState(false);

  const debouncedHandleMouseEnter = debounce(() => setIsHovered(true), 1250);

  const handleOnMouseLeave = () => {
    setIsHovered(false);
    debouncedHandleMouseEnter.clear();
  };

  return (
    <div className={classes.buttonWrapper}>
      <Button
        style={{
          borderRadius: isHovered ? "40px" : "50%",
          padding: "20px",
          margin: 0,
        }}
        onMouseEnter={debouncedHandleMouseEnter}
        onMouseLeave={handleOnMouseLeave}
        onClick={() => props.setIsChatWindowOpen(!props.isChatWindowOpen)}
      >
        {isHovered ? (
          <div className={classes.hoveredButton}>
            <p>Вопрос о работе сайта?</p>
          </div>
        ) : (
          <img src={props.isChatWindowOpen ? cross : supportIcon} />
        )}
      </Button>
    </div>
  );
}

export default ChatButton;
