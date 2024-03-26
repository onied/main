import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import CourseCardsContainer from "../catalog/courseCardsContainer";
import { useState } from "react";

type CourseAuthor = {
  name: string;
};

type CourseCategory = {
  id: string;
  name: string;
};

type CourseCard = {
  isGlowing: Boolean;
  pictureHref: string;
  title: string;
  price: number;
  id: string;
  category: CourseCategory;
  author: CourseAuthor;
};

function ProfileCourses() {
  const profile = useProfile();
  const [coursesList, setCoursesList] = useState<
    Array<CourseCard> | undefined
  >();
  if (profile == null) return <></>;
  return (
    <div className={classes.pageWrapper}>
      <h3 className={classes.pageTitle}>Доступные курсы</h3>
      {coursesList ? (
        <CourseCardsContainer
          coursesList={coursesList}
          owned={true}
        ></CourseCardsContainer>
      ) : (
        <p className={classes.noCourses}>Нет доступных курсов</p>
      )}
    </div>
  );
}

export default ProfileCourses;
