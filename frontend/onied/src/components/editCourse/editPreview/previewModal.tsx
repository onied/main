import { PreviewDto } from "./editPreview";
import classes from "../../../pages/preview/preview.module.css";
import additionalClasses from "./previewModalAdditional.module.css";
import Markdown from "react-markdown";
import CourseProgram from "../../preview/courseProgram/courseProgram";
import PreviewPicture from "../../preview/previewPicture/previewPicture";
import { Link } from "react-router-dom";
import Button from "../../general/button/button";
import AuthorBlock from "../../preview/authorBlock/authorBlock";
import { AllowCertificate } from "../../preview/allowCertifiacte/allowCertificate";

function PreviewForModal(props: any) {
  const previewInfo = props.previewInfo;
  return (
    <div className={classes.previewContainer}>
      <div className={classes.previewLeftBlock}>
        <h2 className={classes.previewTitle}>{previewInfo.title}</h2>
        <div className={classes.additionalInfoLine}>
          <a
            className={classes.additionalInfo}
            href={`/catalog?=category=${previewInfo.category.id}`}
          >
            категория: {previewInfo.category.name}
          </a>
          <span className={classes.additionalInfo}>
            время прохождения: {previewInfo.hoursCount} часов
          </span>
        </div>
        <Markdown>{previewInfo.description}</Markdown>
        {previewInfo.isContentProgramVisible &&
        previewInfo.courseProgram !== null ? (
          <CourseProgram modules={previewInfo.courseProgram!} />
        ) : (
          <></>
        )}
      </div>
      <div className={classes.previewRightBlock}>
        <PreviewPicture
          href={previewInfo.pictureHref}
          isArchived={previewInfo.isArchived}
        />

        <h2 className={classes.price}>{previewInfo.price}</h2>
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
          authorName={previewInfo.courseAuthor.name}
          authorAvatarHref={previewInfo.courseAuthor.avatarHref}
        />
        {previewInfo.hasCertificates ? <AllowCertificate /> : <></>}
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
