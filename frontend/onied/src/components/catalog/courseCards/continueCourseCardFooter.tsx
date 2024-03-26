import Button from "../../general/button/button.jsx";
import classes from "./courseCard.module.css";
import { Link } from "react-router-dom";

function ContinueCourseCardFooter({ courseId }: { courseId: string }) {
  return (
    <div
      className={[classes.courseCardFooter, classes.continueCourse].join(" ")}
    >
      <Link to={"/course/" + courseId} className={classes.continueCourse}>
        <Button style={{ fontSize: "15px", width: "80%" }}>продолжить</Button>
      </Link>
    </div>
  );
}

export default ContinueCourseCardFooter;
