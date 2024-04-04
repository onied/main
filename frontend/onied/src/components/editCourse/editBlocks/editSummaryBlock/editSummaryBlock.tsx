import ButtonGoBack from "../../../general/buttonGoBack/buttonGoBack";
import { useNavigate, useParams } from "react-router-dom";
import { ChangeEvent, useEffect, useState } from "react";
import classes from "./editSummaryBlock.module.css";
import api from "../../../../config/axios";
import Button from "../../../general/button/button";
import MDEditor, { ContextStore } from "@uiw/react-md-editor";
import rehypeSanitize from "rehype-sanitize";
import FileLink from "../../../blocks/summary/fileLink";
import RecycleBinIcon from "../../../../assets/recycleBinIcon.svg";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import InputForm from "../../../general/inputform/inputform";
import DialogActions from "@mui/material/DialogActions";
import Dialog from "@mui/material/Dialog";

type SummaryBlock = {
  id: number;
  title: string;
  blockType: number;
  isCompleted: boolean;
  markdownText: string;
  fileName: string | null;
  fileHref: string | null;
};

function EditSummaryBlockComponent({
  courseId,
  blockId,
}: {
  courseId: number;
  blockId: number;
}) {
  const navigator = useNavigate();
  const [courseAndBlockFound, setCourseAndBlockFound] = useState(false);
  const [currentBlock, setCurrentBlock] = useState<SummaryBlock | undefined>();
  const [fileLoadModalOpen, setFileLoadModalOpen] = useState(false);
  const [newFileName, setNewFileName] = useState<string>("");
  const [newFileHref, setNewFileHref] = useState<string>("");
  const [newFileHrefError, setNewFileHrefError] = useState<string | null>(null);
  const notFound = <h1 style={{ margin: "3rem" }}>Курс или блок не найден.</h1>;
  const parsedCourseId = Number(courseId);
  const parsedBlockId = Number(blockId);

  const saveChanges = () => {
    api.put("", currentBlock).then().catch();
  };

  const saveNewFile = (e: any) => {
    e.preventDefault();
    setCurrentBlock({
      ...currentBlock!,
      fileName: newFileName,
      fileHref: newFileHref,
    });
    api
      .put("", currentBlock)
      .then(() => setFileLoadModalOpen(false))
      .catch();
  };

  const deleteCurrentFile = () => {
    setCurrentBlock({ ...currentBlock!, fileName: null, fileHref: null });
    api.put("", currentBlock).then().catch();
  };

  const onMDChange = (
    value?: string,
    event?: React.ChangeEvent<HTMLTextAreaElement>,
    state?: ContextStore
  ) => {
    if (value === undefined) {
      value = "";
    }
    setCurrentBlock({ ...currentBlock!, markdownText: value! });
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
    <div className={classes.summaryEditWrapper}>
      <div>
        <div className={classes.editHeader}>
          <ButtonGoBack
            onClick={() => navigator("../hierarchy", { relative: "path" })}
          >
            ⟵ к редактированию иерархии
          </ButtonGoBack>
          <div className={classes.title}>{currentBlock!.title}</div>
        </div>
        <MDEditor
          data-color-mode={"light"}
          onChange={onMDChange}
          value={currentBlock!.markdownText}
          previewOptions={{
            rehypePlugins: [[rehypeSanitize]],
          }}
          textareaProps={{
            placeholder: "Введите сюда текст конспекта",
          }}
        />
        <div className={classes.fileAddingContainer}>
          <div className={classes.fileAddingFirstRow}>
            <span>Добавленный файл</span>
            <Button
              style={{ padding: "8px 30px" }}
              onClick={() => setFileLoadModalOpen(true)}
            >
              загрузить файл
            </Button>
          </div>
          {currentBlock!.fileHref ? (
            <div className={classes.fileAddingFileIcon}>
              <FileLink
                fileName={currentBlock!.fileName}
                fileHref={currentBlock!.fileHref}
              />
              <div>
                <img src={RecycleBinIcon} onClick={deleteCurrentFile} />
              </div>
            </div>
          ) : (
            <p className={classes.noFiles}>
              У этого блока нет прикрепленного файла
            </p>
          )}
        </div>

        <div className={classes.line}></div>
        <div className={classes.saveChanges}>
          <Button style={{ padding: "8px 50px" }} onClick={saveChanges}>
            сохранить изменения
          </Button>
        </div>
      </div>
      <Dialog
        open={fileLoadModalOpen}
        onClose={() => setFileLoadModalOpen(false)}
        PaperProps={{
          component: "form",
          onSubmit: saveNewFile,
        }}
      >
        <DialogTitle>Загрузить файл</DialogTitle>
        <DialogContent>
          <DialogContentText>Введите имя файла</DialogContentText>
          <InputForm
            value={newFileName}
            onChange={(e: ChangeEvent<HTMLInputElement>) =>
              setNewFileName(e.target.value)
            }
            style={{ margin: "1rem" }}
            type="text"
          ></InputForm>
          <DialogContentText>Введите ссылку на файл</DialogContentText>

          <InputForm
            value={newFileHref}
            onChange={(e: ChangeEvent<HTMLInputElement>) =>
              setNewFileHref(e.target.value)
            }
            style={{ margin: "1rem" }}
            type="url"
          ></InputForm>
          {newFileHrefError ? <div>{newFileHrefError}</div> : <></>}
        </DialogContent>
        <DialogActions>
          <button type="submit" className={classes.submitNewFileButton}>
            сохранить
          </button>
        </DialogActions>
      </Dialog>
    </div>
  );
}

export default EditSummaryBlockComponent;
