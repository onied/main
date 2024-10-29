import classes from "./chatInput.module.css";
import sendIcon from "../../../assets/sendMessageIcon.svg";
import InputForm from "@onied/components/general/inputform/inputform";
import Button from "@onied/components/general/button/button";
import { ChangeEvent, useState } from "react";

function ChatInput() {
  const [inputText, setInputText] = useState("");

  const sendMessage = () => {
    console.log(inputText);
    setInputText("");
  };

  return (
    <div className={classes.inputWrapper}>
      <InputForm
        className={classes.textArea}
        value={inputText}
        onChange={(e: ChangeEvent<HTMLInputElement>) =>
          setInputText(e.target.value)
        }
      ></InputForm>
      <Button className={classes.sendButton} onClick={sendMessage}>
        <img className={classes.sendIcon} src={sendIcon} />
      </Button>
    </div>
  );
}

export default ChatInput;
