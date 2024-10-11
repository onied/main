import Button from "../../general/button/button";
import classes from "./courseCard.module.css";
import { Link } from "react-router-dom";

function ContinueCourseCardFooter({ courseId }: { courseId: string }) {
  return (
    <div
      className={[classes.courseCardFooter, classes.continueCourse].join(" ")}
    >
      <Link
        to={"/course/" + courseId + "/learn"}
        className={classes.continueCourse}
      >
        <Button style={{ fontSize: "15px", width: "80%" }}>продолжить</Button>
      </Link>
    </div>
  );
}

export default ContinueCourseCardFooter;
