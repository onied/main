import classes from "./chatInput.module.css";
import sendIcon from "../../../assets/sendMessageIcon.svg";
import InputForm from "@onied/components/general/inputform/inputform";
import Button from "@onied/components/general/button/button";
import { ChangeEvent, useState } from "react";
import {
  Badge,
  Dialog,
  DialogContent,
  DialogContentText,
  DialogTitle,
  IconButton,
} from "@mui/material";
import { AttachFile } from "@mui/icons-material";
import FileInputDialog from "@onied/components/general/fileInputDialog/fileInputDialog";
import { FileDto } from "../messageDtos";
import api from "@onied/config/axios";
import CustomBeatLoader from "@onied/components/general/customBeatLoader";

type UploadedFile = {
  link: string;
  filename: string;
};

function ChatInput({
  sendMessageDisabled,
  sendMessageToHub,
}: {
  sendMessageDisabled: boolean;
  sendMessageToHub: (messageContent: string, files: FileDto[]) => void;
}) {
  const [inputText, setInputText] = useState("");
  const [open, setOpen] = useState<boolean>(false);
  const [files, setFiles] = useState<File[]>([]);
  const [uploading, setUploading] = useState<boolean>(false);
  const [error, setError] = useState<string>();

  const sendMessage = () => {
    if (sendMessageDisabled) return;
    let promise = Promise.resolve<UploadedFile[]>([]);
    if (files.length > 0) {
      setUploading(true);
      const formData = new FormData();
      files.forEach((file) => {
        formData.append("files", file);
      });
      promise = api
        .postForm("storage/upload", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        })
        .then((response) => response.data as UploadedFile[]);
    }
    promise
      .then((uploadedFiles) => {
        sendMessageToHub(
          inputText,
          uploadedFiles.map<FileDto>((file) => {
            return { filename: file.filename, fileUrl: file.link };
          })
        );
        setInputText("");
        setFiles([]);
      })
      .catch(() => {
        setError("Could not upload files.");
      })
      .finally(() => {
        setUploading(false);
      });
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
      <Dialog open={uploading}>
        <CustomBeatLoader />
      </Dialog>
      <Dialog open={error !== undefined} onClose={() => setError(undefined)}>
        <DialogTitle>Could not upload files</DialogTitle>
        <DialogContent>
          <DialogContentText>{error}</DialogContentText>
        </DialogContent>
      </Dialog>
    </div>
  );
}

export default ChatInput;
