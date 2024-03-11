import { ReactNode } from "react";
import classes from "./previewPicture.module.css";

type PreviewPictureProps = {
  href: string;
  isArchived: boolean;
};

function PreviewPicture(props: PreviewPictureProps): ReactNode {
  return (
    <div style={{ position: "relative" }}>
      <img className={classes.previewPicture} src={props.href}></img>
      {props.isArchived ? (
        <div className={classes.archived}>
          <span>в архиве</span>
        </div>
      ) : null}
    </div>
  );
}

export default PreviewPicture;
