import classes from "./editVideoBlock.module.css";
import ButtonGoBack from "../../../general/buttonGoBack/buttonGoBack";
import EmbedVideo from "../../../blocks/video/embedVideo";
import InputForm from "../../../general/inputform/inputform";
import { ChangeEvent, useEffect, useState } from "react";
import Button from "../../../general/button/button";
import { useNavigate, useParams } from "react-router-dom";
import api from "../../../../config/axios";
import YoutubeVideoProvider from "../../../blocks/video/youtubeVideoProvider";
import VkVideoProvider from "../../../blocks/video/vkVideoProvider";
import RutubeVideoProvider from "../../../blocks/video/rutubeVideoProvider";
import { BeatLoader } from "react-spinners";

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
];

function EditVideoBlockComponent() {
  const navigator = useNavigate();
  const { courseId, blockId } = useParams();

  const [currentBlock, setCurrentBlock] = useState<
    VideoBlock | null | undefined
  >();
  const [errorLink, setErrorLink] = useState<string>("");

  const notFound = <h1 style={{ margin: "3rem" }}>Курс или блок не найден.</h1>;
  const [isForbid, setIsForbid] = useState(false);
  const forbid = (
    <h1 style={{ margin: "3rem" }}>Вы не можете редактировать данный курс.</h1>
  );

  const parsedCourseId = Number(courseId);
  const parsedBlockId = Number(blockId);

  const validationLink = (link: string) => {
    const embedRegex = embedElements.filter((item) => item.regex.test(link));

    if (embedRegex.length == 0) setErrorLink("Неверный формат ссылки на видео");
    else setErrorLink("");
  };

  const addLink = (e: ChangeEvent<HTMLInputElement>) => {
    validationLink(e.target.value);
    setCurrentBlock({ ...currentBlock!, href: e.target.value });
  };

  const saveChanges = () => {
    api
      .put("courses/" + courseId + "/video/" + blockId + "/edit", currentBlock)
      .catch((error) => {
        if ("response" in error && error.response.status == 404) {
          setCurrentBlock(null);
        } else if ("response" in error && error.response.status == 403) {
          setCurrentBlock(null);
          setIsForbid(true);
        }
      });
  };

  useEffect(() => {
    if (isNaN(parsedCourseId) || isNaN(parsedBlockId)) {
      setCurrentBlock(null);
      return;
    }
    api
      .get("courses/" + courseId + "/CheckEditCourse")
      .then(() => {
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
        if ("response" in error && error.response.status == 404) {
          setCurrentBlock(null);
        } else if ("response" in error && error.response.status == 403) {
          setCurrentBlock(null);
          setIsForbid(true);
        }
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
          onClick={() => navigator("../../hierarchy", { relative: "path" })}
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
        <div className={classes.line}></div>
        <div className={classes.saveChanges}>
          <Button onClick={saveChanges}>сохранить изменения</Button>
        </div>
      </div>
    </>
  );
}

export default EditVideoBlockComponent;
