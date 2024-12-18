import api from "@onied/config/axios";
import classes from "./fileEntry.module.css";

type FileEntryProps = {
  fileName: string;
  objectName: string;
  className: string;
};

export default function FileEntry({
  objectName,
  className,
  fileName,
}: FileEntryProps) {
  const handleClick = async (
    event: React.MouseEvent<HTMLAnchorElement, MouseEvent>
  ) => {
    event.stopPropagation();
    event.preventDefault();
    const response = await api.get("/storage/get/" + objectName);
    const a = document.createElement("a");
    a.href = response.data.presignedUrl;
    a.target = "_blank";
    a.click();
  };
  return (
    <span
      onClick={handleClick}
      onAuxClick={handleClick}
      className={[classes.file, className].join(" ")}
    >
      {fileName}
    </span>
  );
}
