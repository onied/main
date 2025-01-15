import { useEffect, useState } from "react";
import api from "@onied/config/axios";
import classes from "./FileMetadata.module.css";

interface Metadata {
  [key: string]: string;
}

function FileMetadata(props: { fileId?: string }) {
  const [metadata, setMetadata] = useState<Metadata | null>(null);

  useEffect(() => {
    if (!props.fileId) return;

    api
      .get(`/storage/metadata/${props.fileId}`)
      .then((response) => setMetadata(response.data))
      .catch(() => setMetadata(null)); // Suppress errors
  }, [props.fileId]);

  if (!props.fileId || !metadata) return null;

  return (
    <div className={classes.metadataContainer}>
      <h4>Metadata</h4>
      <ul>
        {Object.entries(metadata).map(([key, value]) => (
          <li key={key}>
            <strong>{key}:</strong> {value}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default FileMetadata;
