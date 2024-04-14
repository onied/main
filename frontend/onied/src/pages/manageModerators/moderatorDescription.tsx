import classes from "./manageModerators.module.css";
import { Student } from "./manageModerators";
import Avatar from "react-avatar";

function ModeratorDescription(props: Student) {
  return (
    <>
      <div className={classes.studentWrapper}>
        <Avatar
          name={props.firstName + " " + props.lastName}
          size="64"
          className={classes.profileAvatar}
          src={props.avatarHref ? props.avatarHref : undefined}
        ></Avatar>
        <p className={classes.studentName}>
          {props.firstName + " " + props.lastName}
        </p>
      </div>
    </>
  );
}

export default ModeratorDescription;
