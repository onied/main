import classes from "./chatInput.module.css";
import sendIcon from "../../../assets/sendMessageIcon.svg";
import InputForm from "@onied/components/general/inputform/inputform";
import Button from "@onied/components/general/button/button";
import { ChangeEvent, useState } from "react";
import { Badge, IconButton } from "@mui/material";
import { AttachFile } from "@mui/icons-material";
import FileInputDialog from "@onied/components/general/fileInputDialog/fileInputDialog";

function ChatInput({
  sendMessageDisabled,
  sendMessageToHub,
}: {
  sendMessageDisabled: boolean;
  sendMessageToHub: (messageContent: string) => void;
}) {
  const [inputText, setInputText] = useState("");
  const [open, setOpen] = useState<boolean>(false);
  const [files, setFiles] = useState<File[]>([]);

  const sendMessage = () => {
    if (sendMessageDisabled) return;
    sendMessageToHub(inputText);
    setInputText("");
  };

  return (
    <div className={classes.inputWrapper}>
      <IconButton
        onClick={() => setOpen(true)}
        sx={{ ":hover": { backgroundColor: "unset" } }}
      >
        <Badge badgeContent={files.length}>
          <AttachFile />
        </Badge>
      </IconButton>
      <InputForm
        className={classes.textArea}
        value={inputText}
        onChange={(e: ChangeEvent<HTMLInputElement>) =>
          setInputText(e.target.value)
        }
        disabled={sendMessageDisabled}
      ></InputForm>
      <Button
        className={classes.sendButton}
        onClick={sendMessage}
        disabled={sendMessageDisabled}
      >
        <img className={classes.sendIcon} src={sendIcon} />
      </Button>
      <FileInputDialog
        open={open}
        onClose={() => setOpen(false)}
        files={files}
        setFiles={setFiles}
      ></FileInputDialog>
    </div>
  );
}

export default ChatInput;
