import classes from "../../../pages/preview/preview.module.css";
import additionalClasses from "./previewModalAdditional.module.css";
import Markdown from "react-markdown";
import CourseProgram from "../../preview/courseProgram/courseProgram";
import PreviewPicture from "../../preview/previewPicture/previewPicture";
import { Link } from "react-router-dom";
import Button from "../../general/button/button";
import AuthorBlock from "../../preview/authorBlock/authorBlock";
import { AllowCertificate } from "../../preview/allowCertifiacte/allowCertificate";

type PreviewModalProps = {
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
  isProgramVisible: boolean;
  courseProgram: Array<string> | undefined;
  onCloseClick: (isOpen: boolean) => void;
};

function PreviewForModal(props: PreviewModalProps) {
  return (
    <div className={classes.previewContainer}>
      <div className={classes.previewLeftBlock}>
        <h2 className={classes.previewTitle}>{props.title}</h2>
        <div className={classes.additionalInfoLine}>
          <a
            className={classes.additionalInfo}
            href={`/catalog?=category=${props.category.id}`}
          >
            категория: {props.category.name}
          </a>
          <span className={classes.additionalInfo}>
            время прохождения: {props.hoursCount} часов
          </span>
        </div>
        <Markdown>{props.description}</Markdown>
        {props.isProgramVisible && props.courseProgram !== null ? (
          <CourseProgram modules={props.courseProgram!} />
        ) : (
          <></>
        )}
      </div>
      <div className={classes.previewRightBlock}>
        <PreviewPicture
          href={props.pictureHref}
          isArchived={props.isArchived}
        />

        <h2 className={classes.price}>{props.price}</h2>
        <Link to="learn">
          <Button
            style={{
              width: "100%",
              fontSize: "20pt",
              textDecorations: "none",
              backgroundColor: "#9715d3",
            }}
          >
            купить
          </Button>
        </Link>
        <AuthorBlock
          authorName={props.courseAuthor.name}
          authorAvatarHref={props.courseAuthor.avatarHref}
        />
        {props.hasCertificates ? <AllowCertificate /> : <></>}
      </div>
      <div
        className={additionalClasses.close}
        onClick={() => {
          props.onCloseClick(false);
        }}
      ></div>
    </div>
  );
}

export default PreviewForModal;
