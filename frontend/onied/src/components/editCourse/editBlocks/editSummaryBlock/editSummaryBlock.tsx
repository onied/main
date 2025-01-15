import ButtonGoBack from "../../../general/buttonGoBack/buttonGoBack";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import classes from "./editSummaryBlock.module.css";
import api from "../../../../config/axios";
import Button from "../../../general/button/button";
import MDEditor, { ContextStore } from "@uiw/react-md-editor";
import rehypeSanitize from "rehype-sanitize";
import FileLink from "../../../blocks/summary/fileLink";
import RecycleBinIcon from "../../../../assets/recycleBinIcon.svg";
import NotFound from "../../../general/responses/notFound/notFound";
import { BeatLoader } from "react-spinners";
import Forbid from "../../../general/responses/forbid/forbid";
import {
    booksContext,
    documentsContext,
    exerciseMaterialsContext
} from "@onied/components/general/fileUploading/predefinedFileContexts";
import FileUploadingDialog from "@onied/components/general/fileUploading/fileUploadingDialog";

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
  const [currentBlock, setCurrentBlock] = useState<
    SummaryBlock | null | undefined
  >();
  const [fileLoadModalOpen, setFileLoadModalOpen] = useState(false);
  const [_, setFileId] = useState<string | null>(null);

  const notFound = <NotFound>Курс или блок не найден.</NotFound>;
  const [isForbid, setIsForbid] = useState(false);
  const forbid = <Forbid>Вы не можете редактировать данный курс.</Forbid>;

  const parsedCourseId = Number(courseId);
  const parsedBlockId = Number(blockId);

  const saveChanges = () => {
    sendingBlock(currentBlock!);
  };

  const deleteCurrentFile = () => {
    setCurrentBlock({ ...currentBlock!, fileName: null, fileHref: null });
    sendingBlock({ ...currentBlock!, fileName: null, fileHref: null });
  };

  const sendingBlock = (block: SummaryBlock) => {
    return api
      .put("courses/" + courseId + "/edit/summary/" + blockId, block)
      .catch((error) => {
        if ("response" in error && error.response.status == 404) {
          setCurrentBlock(null);
        } else if ("response" in error && error.response.status == 403) {
          setCurrentBlock(null);
          setIsForbid(true);
        }
      });
  };

  const onMDChange = (
    value?: string,
    _?: React.ChangeEvent<HTMLTextAreaElement>,
    __?: ContextStore
  ) => {
    if (value === undefined) {
      value = "";
    }
    setCurrentBlock({ ...currentBlock!, markdownText: value! });
  };

  useEffect(() => {
    if (isNaN(parsedCourseId) || isNaN(parsedBlockId)) {
      setCurrentBlock(null);
      return;
    }
    api
      .get("courses/" + courseId + "/edit/check-edit-course")
      .then((_) => {
        api
          .get("courses/" + courseId + "/summary/" + blockId)
          .then((response) => {
            console.log(response.data);
            setCurrentBlock(response.data);
          })
          .catch((error) => {
            if ("response" in error && error.response.status == 404) {
              setCurrentBlock(null);
            }
          });
      })
      .catch((error) => {
        if (error.response?.status === 403) setIsForbid(true);
      });
  }, []);

  if (isNaN(parsedCourseId) && isNaN(parsedBlockId)) {
    console.log(courseId);
    console.log(blockId);
    return notFound;
  }

  if (isForbid) return forbid;

  if (currentBlock === undefined)
    return (
      <BeatLoader
        color="var(--accent-color)"
        style={{ margin: "3rem" }}
      ></BeatLoader>
    );

  if (currentBlock === null) return notFound;

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
            <FileUploadingDialog
              open={fileLoadModalOpen}
              onClose={() => setFileLoadModalOpen(false)}
              setFileId={setFileId}
              contexts={[documentsContext, exerciseMaterialsContext, booksContext]}
            ></FileUploadingDialog>
          </div>
          {currentBlock!.fileHref ? (
            <div className={classes.fileAddingFileIcon}>
              <FileLink
                fileName={currentBlock!.fileName!}
                fileHref={currentBlock!.fileHref}
              />
              <div>
                <img
                  src={RecycleBinIcon}
                  onClick={deleteCurrentFile}
                  alt="убрать файл"
                />
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
    </div>
  );
}

export default EditSummaryBlockComponent;
