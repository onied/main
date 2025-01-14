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
  context: MetadataContext;
};

function FileUploadingDialog(props: FileUploadingDialogProps) {
  const [file, setFile] = useState<File | null>(null);
  const [formData, setFormData] = useState<{ [key: string]: string }>({});
  const [errors, setErrors] = useState<{ [key: string]: string }>({});

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
  };

  const handleSubmit = () => {
    if (file && formData) {
      //onSubmit(file, description);
    }
    props.onClose();
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
                  onChange={(e) => handleFieldChange(e, field.name)}
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
          <Button onClick={handleSubmit} disabled={!file || !formData}>
            Загрузить
          </Button>
        </DialogActions>
      </div>
    </Dialog>
  );
}

export default FileUploadingDialog;
