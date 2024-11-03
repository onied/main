import classes from "./chatButton.module.css";
import supportIcon from "../../../assets/supportIcon.svg";
import cross from "../../../assets/whiteCross.svg";
import Button from "@onied/components/general/button/button";
import { Badge, BadgeProps, debounce, styled } from "@mui/material";
import { useState } from "react";

type ChatButtonProps = {
  isChatWindowOpen: boolean;
  setIsChatWindowOpen: (isChatWindowOpen: boolean) => void;
  unreadCount: number;
};

const StyledBadge = styled(Badge)<BadgeProps>(() => ({
  "& .MuiBadge-badge": {
    right: 10,
    top: 10,
    border: `2px solid white`,
    "background-color": "#ED4956",
    "font-size": "14pt",
    "font-weight": "800",
    "border-radius": "50%",
    "aspect-ratio": "1/1",
    padding: "0.8rem",
    color: "white",
  },
}));

function ChatButton(props: ChatButtonProps) {
  const [isHovered, setIsHovered] = useState(false);

  const debouncedHandleMouseEnter = debounce(() => setIsHovered(true), 1250);

  const handleOnMouseLeave = () => {
    setIsHovered(false);
    debouncedHandleMouseEnter.clear();
  };

  return (
    <div className={classes.buttonWrapper}>
      <StyledBadge badgeContent={props.unreadCount}>
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
      </StyledBadge>
    </div>
  );
}

export default ChatButton;
