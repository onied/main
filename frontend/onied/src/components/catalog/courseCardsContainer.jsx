import GeneralCourseCard from "./courseCards/generalCourseCard.jsx";
import classes from "./courseCardsContainer.module.css";
import { BeatLoader } from "react-spinners";

function CourseCardsContainer({ coursesList, owned = false }) {
  if (coursesList == undefined)
    return (
      <BeatLoader
        color="var(--accent-color)"
        style={{ margin: "5rem" }}
      ></BeatLoader>
    );
  return (
    <div className={classes.courseCardsContainer}>
      {coursesList.map((courseInfo, index) => (
        <GeneralCourseCard card={courseInfo} owned={owned} key={index} />
      ))}
    </div>
  );
}

export default CourseCardsContainer;
