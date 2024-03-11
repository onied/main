import GeneralCourseCard from "./courseCards/generalCourseCard.jsx";
import classes from "./courseCardsContainer.module.css";

function CourseCardsContainer({ coursesList }) {
  return (
    <div className={classes.courseCardsContainer}>
      {coursesList.map((courseInfo, index) => (
        <GeneralCourseCard card={courseInfo} key={index} />
      ))}
    </div>
  );
}

export default CourseCardsContainer;
