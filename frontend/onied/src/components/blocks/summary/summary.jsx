import { useEffect, useState } from "react";
import Markdown from "react-markdown";
import classes from "./summary.module.css";
import FileLink from "./fileLink";
import api from "../../../config/axios";
import CustomBeatLoader from "../../general/customBeatLoader";

function SummaryDto(title, markdownText, fileName, fileHref) {
  this.title = title;
  this.markdownText = markdownText;
  this.file = {
    name: fileName,
    href: fileHref,
  };
}

function Summary({ courseId, blockId }) {
  const [summary, setSummary] = useState();
  const [found, setFound] = useState();

  useEffect(() => {
    setFound(undefined);
    api
      .get("courses/" + courseId + "/summary/" + blockId)
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setSummary(
          new SummaryDto(
            response.data.title,
            response.data.markdownText,
            response.data.fileName,
            response.data.fileHref
          )
        );
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, []);

  if (found == null) return <CustomBeatLoader />;
  if (!found) return <></>;
  return (
    <>
      <h2>{summary.title}</h2>
      <Markdown className={classes.markdownText}>
        {summary.markdownText}
      </Markdown>
      {summary.file.href == null ? null : (
        <FileLink fileName={summary.file.name} fileHref={summary.file.href} />
      )}
    </>
  );
}

export default Summary;
