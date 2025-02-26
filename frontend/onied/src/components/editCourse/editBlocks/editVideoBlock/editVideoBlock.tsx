import classes from "./editVideoBlock.module.css";
import ButtonGoBack from "../../../general/buttonGoBack/buttonGoBack";
import InputForm from "../../../general/inputform/inputform";
import { ChangeEvent, useEffect, useState } from "react";
import Button from "../../../general/button/button";
import { useNavigate } from "react-router-dom";
import api from "../../../../config/axios";
import NotFound from "../../../general/responses/notFound/notFound";
import { BeatLoader } from "react-spinners";
import Forbid from "../../../general/responses/forbid/forbid";

import EmbedVideo from "@onied/components/blocks/video/embedVideo";
import YoutubeVideoProvider from "@onied/components/blocks/video/providers/youtubeVideoProvider";
import VkVideoProvider from "@onied/components/blocks/video/providers/vkVideoProvider";
import RutubeVideoProvider from "@onied/components/blocks/video/providers/rutubeVideoProvider";
import FileUploadingDialog from "@onied/components/general/fileUploading/fileUploadingDialog";
import {
  audioContext,
  videosContext,
} from "@onied/components/general/fileUploading/predefinedFileContexts";
import FileVideoProvider from "@onied/components/blocks/video/providers/fileProvider";
import FileMetadata from "@onied/components/general/fileMetadata/fileMetadata";

type VideoBlock = {
  id: string;
  title: string;
  href: string;
  blockType: string;
  isCompleted: boolean;
};

const embedElements = [
  new YoutubeVideoProvider(),
  new VkVideoProvider(),
  new RutubeVideoProvider(),
  new FileVideoProvider(),
];

function EditVideoBlockComponent({
  courseId,
  blockId,
}: {
  courseId: number;
  blockId: number;
}) {
  const navigator = useNavigate();

  const [currentBlock, setCurrentBlock] = useState<
    VideoBlock | null | undefined
  >();
  const [errorLink, setErrorLink] = useState<string>(
    "Неверный формат ссылки на видео"
  );
  const [isFileUploadDialogOpen, setIsFileUploadDialogOpen] =
    useState<boolean>(false);

  const notFound = <NotFound>Курс или блок не найден.</NotFound>;
  const [isForbid, setIsForbid] = useState(false);
  const forbid = <Forbid>Вы не можете редактировать данный курс.</Forbid>;

  const parsedCourseId = Number(courseId);
  const parsedBlockId = Number(blockId);

  const validationLink = (link: string) => {
    const embedRegex = embedElements.filter((item) => item.regex.test(link));
    console.log(link);
    if (embedRegex.length == 0 || link === null)
      setErrorLink("Неверный формат ссылки на видео");
    else setErrorLink("");
  };

  const addLink = (e: ChangeEvent<HTMLInputElement>) => {
    validationLink(e.target.value);
    setCurrentBlock({ ...currentBlock!, href: e.target.value });
  };

  const saveChanges = () => {
    api
      .put("courses/" + courseId + "/edit/video/" + blockId, currentBlock)
      .catch((error) => {
        if ("response" in error && error.response.status == 404) {
          setCurrentBlock(null);
        } else if ("response" in error && error.response.status == 403) {
          setCurrentBlock(null);
          setIsForbid(true);
        }
      });
  };

  const setFileId = (id: string) => {
    addLink({ target: { value: id } } as ChangeEvent<HTMLInputElement>);
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
          .get("courses/" + courseId + "/video/" + blockId)
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

  useEffect(() => {
    if (currentBlock && currentBlock.href) {
      validationLink(currentBlock.href);
    }
  }, [currentBlock]);

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
    <>
      <div className={classes.container}>
        <ButtonGoBack
          onClick={() => navigator("../hierarchy", { relative: "path" })}
        >
          ⟵ к редактированию иерархии
        </ButtonGoBack>
        <div className={classes.title}>{currentBlock!.title}</div>
        {!errorLink && <EmbedVideo href={currentBlock!.href}></EmbedVideo>}
        <p className={classes.addingLinkText}>
          Вы можете {currentBlock!.href ? "изменить" : "добавить"} видео,
          добавив ссылку на него с YouTube, Rutube или VK Видео
        </p>
        <InputForm
          placeholder="Вставьте сюда ссылку на видео"
          value={currentBlock!.href}
          style={{ width: "60%" }}
          onChange={addLink}
        />
        {errorLink && <span className={classes.errorMessage}>{errorLink}</span>}
        <div>
          <p className={classes.addingLinkText}>
            Или загрузите свой файл на платформу
          </p>
          <FileUploadingDialog
            open={isFileUploadDialogOpen}
            onClose={() => setIsFileUploadDialogOpen(false)}
            setFileId={setFileId}
            contexts={[videosContext, audioContext]}
          ></FileUploadingDialog>
          <FileMetadata fileId={currentBlock.href}></FileMetadata>
          <Button onClick={() => setIsFileUploadDialogOpen(true)}>
            Загрузить
          </Button>
        </div>
        <div className={classes.line}></div>
        <div className={classes.saveChanges}>
          <Button onClick={saveChanges}>сохранить изменения</Button>
        </div>
      </div>
    </>
  );
}

export default EditVideoBlockComponent;
