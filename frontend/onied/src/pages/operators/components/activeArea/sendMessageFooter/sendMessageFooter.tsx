import Config from "@onied/config/config";
import classes from "./sendMessageFooter.module.css";

import InputFormArea from "@onied/components/general/inputform/inputFormArea";
import { useAppSelector } from "@onied/hooks";
import useSignalR from "@onied/hooks/signalr";

import { MouseEventHandler, useState } from "react";
import { chatHubOperatorConnection } from "@onied/pages/operators/chatHubOperatorConnection";
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
import { MessageFile } from "@onied/types/chat";
import CustomBeatLoader from "@onied/components/general/customBeatLoader";
import api from "@onied/config/axios";

type UploadedFile = {
  objectName: string;
  filename: string;
};

export default function SendMessageFooter() {
  const [open, setOpen] = useState<boolean>(false);
  const [files, setFiles] = useState<File[]>([]);
  const chats = useAppSelector((state) => state.chats);
  const [message, setMessage] = useState<string>("");
  const [uploading, setUploading] = useState<boolean>(false);
  const [error, setError] = useState<string>();
  const { connection } = useSignalR(
    Config.BaseURL.replace(/\/$/, "") + "/chat/hub"
  );

  const sendMessage = () => {
    if (!connection || !chats.currentChatId) return;
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
        if (!connection || !chats.currentChatId) return;
        const chatOperator = chatHubOperatorConnection(connection);
        chatOperator.send.SendMessageToChat(
          chats.currentChatId,
          message,
          uploadedFiles.map<MessageFile>((file) => {
            return { filename: file.filename, fileUrl: file.objectName };
          })
        );
        setFiles([]);
        setMessage("");
      })
      .catch(() => {
        setError("Could not upload files.");
      })
      .finally(() => {
        setUploading(false);
      });
  };

  if (!connection) return <></>;
  return (
    <div className={classes.chatFooter}>
      <IconButton
        onClick={() => setOpen(true)}
        sx={{ ":hover": { backgroundColor: "unset" } }}
      >
        <Badge badgeContent={files.length}>
          <AttachFile />
        </Badge>
      </IconButton>
      <InputFormArea
        onInput={(e: any) => setMessage(e.target.value)}
        value={message}
        style={{ width: "100%", height: "40px", resize: "none" }}
      />
      <SendButton onClick={sendMessage} />
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

const SendButton = ({ onClick }: { onClick?: MouseEventHandler }) => {
  return (
    <svg
      className={classes.sendButton}
      onClick={onClick}
      width="40"
      height="40"
      viewBox="0 0 40 40"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <rect width="40" height="40" rx="20" fill="#9715D3" />
      <path
        d="M12.71 14.2706L24.03 10.4973C29.11 8.80396 31.87 11.5773 30.19 
                16.6573L26.4167 27.9772C23.8833 35.5906 19.7233 35.5906 17.19 27.9772L16.07 
                24.6172L12.71 23.4972C5.09667 20.9639 5.09667 16.8173 12.71 14.2706Z"
        stroke="white"
        strokeWidth="4"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
      <path
        d="M16.3233 24.0441L21.0967 19.2574"
        stroke="white"
        strokeWidth="4"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  );
};
