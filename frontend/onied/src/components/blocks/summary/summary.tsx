import { useEffect, useState } from "react";
import Markdown from "react-markdown";
import api from "@onied/config/axios";
import CustomBeatLoader from "@onied/components/general/customBeatLoader";
import { SummaryBlock } from "@onied/types/block";
import classes from "./summary.module.css";
import FileMetadata from "@onied/components/general/fileMetadata/fileMetadata";
import FileEntrySummary from "@onied/components/general/fileEntrySummary/fileEntrySummary";

type props = {
  courseId: number;
  blockId: number;
};

function Summary({ courseId, blockId }: props) {
  const [summary, setSummary] = useState<SummaryBlock>();
  const [found, setFound] = useState<boolean>();

  useEffect(() => {
    api
      .get("courses/" + courseId + "/summary/" + blockId)
      .then((response) => {
        const summaryResponse: SummaryBlock = response.data!;
        console.log(summaryResponse);
        setFound(true);
        setSummary(summaryResponse);
      })
      .catch((error) => {
        console.log(error);
        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, []);

  if (found === undefined) return <CustomBeatLoader />;
  if (!found) return <></>;
  return (
    <>
      <h2 data-testid="markdown-summary-title">{summary!.title}</h2>
      <Markdown className={classes.markdownText}>
        {summary!.markdownText}
      </Markdown>
      {summary!.fileName == null || summary!.fileHref == null ? null : (
        <>
          <FileEntrySummary
            fileName={summary!.fileName}
            objectName={summary!.fileHref}
          />
          <FileMetadata fileId={summary!.fileHref}></FileMetadata>
        </>
      )}
    </>
  );
}

export default Summary;
