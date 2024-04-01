import ButtonGoBack from "../../../general/buttonGoBack/buttonGoBack";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import classes from "./editSummaryBlock.module.css";
import api from "../../../../config/axios";
import Button from "../../../general/button/button";
import MDEditor from "@uiw/react-md-editor";
import rehypeSanitize from "rehype-sanitize";
import FileLink from "../../../blocks/summary/fileLink";
import RecycleBinIcon from "../../../../assets/recycleBinIcon.svg";

function EditSummaryBlockComponent() {
  const navigator = useNavigate();
  const { courseId, blockId } = useParams();
  const [courseAndBlockFound, setCourseAndBlockFound] = useState(false);
  const [currentBlock, setCurrentBlock] = useState({
    title: "",
    markdownText: "",
    fileName: "",
    fileHref: "",
  });
  const notFound = <h1 style={{ margin: "3rem" }}>Курс или блок не найден.</h1>;
  const parsedCourseId = Number(courseId);
  const parsedBlockId = Number(blockId);

  const saveChanges = () => {
    api.post("", currentBlock).then().catch();
  };

  useEffect(() => {
    if (isNaN(parsedCourseId) || isNaN(parsedBlockId)) {
      setCourseAndBlockFound(false);
      return;
    }
    api
      .get("courses/" + courseId + "/summary/" + blockId)
      .then((response) => {
        console.log(response.data);
        setCourseAndBlockFound(true);
        setCurrentBlock(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setCourseAndBlockFound(false);
        }
      });
  }, []);

  if (isNaN(parsedCourseId) && isNaN(parsedBlockId)) {
    console.log(courseId);
    console.log(blockId);
    return notFound;
  }

  if (!courseAndBlockFound) return notFound;

  return (
    <>
      <div>
        <div className={classes.editHeader}>
          <ButtonGoBack
            onClick={() => navigator("../../hierarchy", { relative: "path" })}
          >
            ⟵ к редактированию иерархии
          </ButtonGoBack>
          <div className={classes.title}>{currentBlock.title}</div>
        </div>
        <MDEditor
          value={currentBlock.markdownText}
          previewOptions={{
            rehypePlugins: [[rehypeSanitize]],
          }}
        />
        <div className={classes.fileAddingContainer}>
          <div className={classes.fileAddingFirstRow}>
            <span>Добавленный файл</span>
            <Button style={{ padding: "8px 30px" }}>загрузить файл</Button>
          </div>
          <div className={classes.fileAddingFileIcon}>
            <FileLink
              fileName={currentBlock.fileName}
              fileHref={currentBlock.fileHref}
            />
            <img src={RecycleBinIcon} />
          </div>
        </div>

        <div className={classes.line}></div>
        <div className={classes.saveChanges}>
          <Button style={{ padding: "8px 50px" }} onClick={saveChanges}>
            сохранить изменения
          </Button>
        </div>
      </div>
    </>
  );
}

export default EditSummaryBlockComponent;
