import classes from "./fileUploadingDialog.module.css";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Button,
  TextField,
  Box,
} from "@mui/material";
import { MetadataContext } from "@onied/components/general/fileUploading/predefinedFileContexts";
import { useState } from "react";

type FileUploadingDialogProps = {
  open: boolean;
  onClose: () => void;
  context: MetadataContext;
};

function FileUploadingDialog(props: FileUploadingDialogProps) {
  const [file, setFile] = useState<File | null>(null);
  const [formData, setFormData] = useState<{ [key: string]: string }>({});

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const selectedFile = e.target.files ? e.target.files[0] : null;
    setFile(selectedFile);
  };

  const handleFieldChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
    name: string
  ) => {
    setFormData((prevState) => ({
      ...prevState,
      [name]: e.target.value,
    }));
  };

  const handleSubmit = () => {
    if (file) {
      //onSubmit(file, description);
    }
    props.onClose();
  };

  return (
    <Dialog open={props.open} onClose={props.onClose}>
      <DialogTitle>Upload File</DialogTitle>
      <DialogContent>
        <Box sx={{ mb: 2 }}>
          <input
            type="file"
            accept={props.context.types.join(" ")}
            onChange={handleFileChange}
            style={{ marginBottom: "16px" }}
          />
        </Box>
        {props.context.fields.map((field) => (
          <TextField
            key={field.name}
            label={field.label}
            fullWidth
            value={formData[field.name] || ""}
            onChange={(e) => handleFieldChange(e, field.name)}
            variant="outlined"
            margin="normal"
          />
        ))}
      </DialogContent>
      <DialogActions>
        <Button onClick={props.onClose}>
          Cancel
        </Button>
        <Button
          onClick={handleSubmit}
          disabled={!file || !formData}
        >
          Submit
        </Button>
      </DialogActions>
    </Dialog>
  );
}

export default FileUploadingDialog;
