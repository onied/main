import classes from "./fileUploadingDialog.module.css";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Box,
} from "@mui/material";
import { MetadataContext } from "@onied/components/general/fileUploading/predefinedFileContexts";
import { useState } from "react";
import InputForm from "@onied/components/general/inputform/inputform";
import Button from "@onied/components/general/button/button";

type FileUploadingDialogProps = {
  open: boolean;
  onClose: () => void;
  setFileId: (id: string) => void;
  context: MetadataContext;
};

function FileUploadingDialog(props: FileUploadingDialogProps) {
  const [file, setFile] = useState<File | null>(null);
  const [formData, setFormData] = useState<{ [key: string]: string }>({});
  const [errors, setErrors] = useState<{ [key: string]: string }>({});
  const [uploading, setUploading] = useState<boolean>(false);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const selectedFile = e.target.files ? e.target.files[0] : null;
    setFile(selectedFile);
  };

  const handleFieldChange = (
    e: React.ChangeEvent<HTMLInputElement>,
    name: string
  ) => {
    setFormData((prevState) => ({
      ...prevState,
      [name]: e.target.value,
    }));

    const field = props.context.fields.find((field) => field.name === name);
    if (field) {
      const regex = new RegExp(field.validRegex);
      if (!regex.test(e.target.value)) {
        setErrors((prevErrors) => ({
          ...prevErrors,
          [name]: `Поле ${field.label} неверное.`,
        }));
        console.log(errors);
      } else {
        setErrors((prevErrors) => {
          const { [name]: _, ...rest } = prevErrors;
          return rest;
        });
      }
    }
  };

  const clearDialogData = () => {
    setFile(null);
    setFormData({});
    setErrors({});
    setUploading(false);
  };

  const handleSubmit = () => {
    if (file && formData) {
      setUploading(true);
      setTimeout(() => {
        const generatedFileId = crypto.randomUUID(); // Генерация случайного fileId
        props.setFileId(generatedFileId);
        clearDialogData();
        props.onClose();
        setUploading(false);
      }, 2000);
    }
  };

  return (
    <Dialog open={props.open} onClose={props.onClose} fullWidth>
      <div className={classes.dialogContentsWrapper}>
        <DialogTitle>Загрузить файл</DialogTitle>
        <DialogContent>
          <Box sx={{ mb: 2 }}>
            <input
              type="file"
              accept={props.context.types.join(", ")}
              onChange={handleFileChange}
              style={{ marginBottom: "16px" }}
            />
          </Box>
          <div className={classes.metadataForm}>
            {props.context.fields.map((field) => (
              <>
                <span>{field.label}</span>
                <InputForm
                  className={classes.input}
                  key={field.name}
                  label={field.label}
                  value={formData[field.name] || ""}
                  onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleFieldChange(e, field.name)}
                />
                {errors[field.name] ? (
                  <>
                    <div></div>
                    <p className={classes.errorMessage}>{errors[field.name]}</p>
                  </>
                ) : null}
              </>
            ))}
          </div>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() => {
              clearDialogData();
              props.onClose();
            }}
          >
            Отмена
          </Button>
          <Button
            onClick={handleSubmit}
            disabled={
              !file ||
              !(
                Object.values(formData).length == props.context.fields.length
              ) ||
              uploading
            }
          >
            {uploading ? "Загружается..." : "Загрузить"}
          </Button>
        </DialogActions>
      </div>
    </Dialog>
  );
}

export default FileUploadingDialog;
