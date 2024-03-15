import Button from "../../general/button/button.jsx";
import classes from "./courseCard.module.css";
import { numberWithSpaces } from "../utils.js";
import { Link } from "react-router-dom";

function PaidCourseCardFooter({ price, courseId }) {
  return (
    <div className={classes.courseCardFooter}>
      <span>{numberWithSpaces(price)} ₽</span>
      <Link to={"/course/" + courseId}>
        <Button style={{ fontSize: "15px" }}>купить</Button>
      </Link>
    </div>
  );
}

export default PaidCourseCardFooter;
