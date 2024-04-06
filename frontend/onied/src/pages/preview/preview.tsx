import { ReactNode, useEffect, useState } from "react";
import Markdown from "react-markdown";

import classes from "./preview.module.css";
import { AllowCertificate } from "../../components/preview/allowCertifiacte/allowCertificate";
import PreviewPicture from "../../components/preview/previewPicture/previewPicture";
import CourseProgram from "../../components/preview/courseProgram/courseProgram";
import Button from "../../components/general/button/button";
import AuthorBlock from "../../components/preview/authorBlock/authorBlock";
import { Link, useParams } from "react-router-dom";
import BeatLoader from "react-spinners/BeatLoader";
import api from "../../config/axios";
import NotFound from "../../components/general/responses/notFound/notFound";

type PreviewDto = {
  title: string;
  pictureHref: string;
  description: string;
  hoursCount: number;
  price: number;
  category: {
    id: number;
    name: string;
  };
  courseAuthor: {
    name: string;
    avatarHref: string;
  };
  isArchived: boolean;
  hasCertificates: boolean;
  courseProgram: Array<string> | undefined;
};

function Preview(): ReactNode {
  const { courseId } = useParams();
  const [dto, setDto] = useState<PreviewDto | undefined>();
  const [found, setFound] = useState<boolean | undefined>();
  const notFound = <NotFound>Курс не найден.</NotFound>;

  const id = Number(courseId);
  if (isNaN(id)) return notFound;

  useEffect(() => {
    setFound(undefined);
    api
      .get("courses/" + id)
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setDto(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, [courseId]);

  if (found == undefined || dto == undefined)
    return <BeatLoader color="var(--accent-color)"></BeatLoader>;
  if (!found) return notFound;
  return (
    <div className={classes.previewContainer}>
      <div className={classes.previewLeftBlock}>
        <h2 className={classes.previewTitle}>{dto.title}</h2>
        <div className={classes.additionalInfoLine}>
          <a
            className={classes.additionalInfo}
            href={`/catalog?=category=${dto.category.id}`}
          >
            категория: {dto.category.name}
          </a>
          <span className={classes.additionalInfo}>
            время прохождения: {dto.hoursCount} часов
          </span>
        </div>
        <Markdown>{dto.description}</Markdown>
        {dto.courseProgram == null ? null : (
          <CourseProgram modules={dto.courseProgram} />
        )}
      </div>
      <div className={classes.previewRightBlock}>
        <PreviewPicture href={dto.pictureHref} isArchived={dto.isArchived} />
        <h2 className={classes.price}>{dto.price}</h2>
        <Link to="learn">
          <Button
            style={{ width: "100%", fontSize: "20pt", textDecorations: "none" }}
          >
            купить
          </Button>
        </Link>
        <AuthorBlock
          authorName={dto.courseAuthor.name}
          authorAvatarHref={dto.courseAuthor.avatarHref}
        />
        {dto.hasCertificates ? <AllowCertificate /> : <></>}
      </div>
    </div>
  );
}

export default Preview;
