import api from "@onied/config/axios";
import classes from "./fileEntrySummary.module.css";

type FileEntryProps = {
  fileName: string;
  objectName: string;
};

export default function FileEntrySummary({
  objectName,
  fileName,
}: FileEntryProps) {
  const handleClick = async (
    event: React.MouseEvent<HTMLAnchorElement, MouseEvent>
  ) => {
    event.stopPropagation();
    event.preventDefault();
    const response = await api.get(`/storage/download-url/${objectName}`);
    const link = document.createElement("a");
    link.href = response.data.presignedUrl;
    link.target = "_blank";
    link.click();
  };
  return (
    <span
      onClick={handleClick}
      onAuxClick={handleClick}
      className={classes.file}
    >
      {fileName}
    </span>
  );
}
