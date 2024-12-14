import {
  Avatar,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
} from "@mui/material";
import DeleteIcon from "@mui/icons-material/Delete";
import { InsertDriveFile } from "@mui/icons-material";
import Dropzone from "react-dropzone";
import classes from "./fileInputDialog.module.css";
import { useEffect, useState } from "react";

type FileInputDialogProps = {
  open: boolean;
  onClose: () => void;
  files: File[];
  setFiles: (files: File[]) => void;
};

export default function FileInputDialog(props: FileInputDialogProps) {
  const [unsavedFiles, setUnsavedFiles] = useState<File[]>(props.files);
  const handleDeleteFile = (index: number) => {
    setUnsavedFiles(unsavedFiles.filter((_, ind) => ind != index));
  };

  const addFilesToList = (files: File[]) => {
    setUnsavedFiles(
      unsavedFiles
        .concat(files)
        .filter(
          (file, index, array) =>
            array.findIndex(
              (val) =>
                val.name == file.name &&
                val.lastModified == file.lastModified &&
                val.size == file.size
            ) == index
        )
        .slice(0, 10)
    );
  };

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    props.setFiles(unsavedFiles);
    props.onClose();
  };

  useEffect(() => {
    setUnsavedFiles(props.open ? props.files : []);
  }, [props.open]);

  return (
    <>
      <Dialog
        open={props.open}
        onClose={props.onClose}
        PaperProps={{
          component: "form",
          onSubmit: handleSubmit,
        }}
      >
        <DialogTitle>Загрузка файлов</DialogTitle>
        <DialogContent>
          <List dense={true}>
            {unsavedFiles.map((file, index) => {
              return (
                <>
                  <ListItem
                    secondaryAction={
                      <IconButton
                        edge="end"
                        aria-label="delete"
                        onClick={() => handleDeleteFile(index)}
                      >
                        <DeleteIcon />
                      </IconButton>
                    }
                  >
                    <ListItemAvatar>
                      <Avatar>
                        <InsertDriveFile />
                      </Avatar>
                    </ListItemAvatar>
                    <ListItemText primary={file.name} />
                  </ListItem>
                </>
              );
            })}
          </List>
          <Dropzone
            onDrop={addFilesToList}
            maxFiles={10}
            maxSize={5 * 1024 * 1024}
          >
            {({ isDragAccept, isDragReject, getRootProps, getInputProps }) => (
              <section>
                <div
                  {...getRootProps({
                    className: [
                      classes.dropzone,
                      isDragAccept ? classes.accept : "",
                      isDragReject ? classes.reject : "",
                    ].join(" "),
                  })}
                >
                  <input {...getInputProps()} />
                  <p>Перетащите сюда файлы</p>
                </div>
              </section>
            )}
          </Dropzone>
        </DialogContent>
        <DialogActions className={classes.footer}>
          <Button onClick={props.onClose} className={classes.button}>
            Отменить
          </Button>
          <Button type="submit" className={classes.button}>
            Принять
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
