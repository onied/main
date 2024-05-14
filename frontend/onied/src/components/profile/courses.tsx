import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import CourseCardsContainer from "../catalog/courseCardsContainer";
import { useEffect, useState } from "react";
import api from "../../config/axios";
import CustomBeatLoader from "../general/customBeatLoader";

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
  const [profile, _] = useProfile();
  const [coursesList, setCoursesList] = useState<
    Array<CourseCard> | undefined
  >();
  useEffect(() => {
    if (profile == null) return;
    api
      .get("/account/courses")
      .then((response) => {
        console.log(response);
        setCoursesList(response.data);
      })
      .catch(() => {
        setCoursesList([]);
      });
  }, [profile]);
  if (profile == null) return <></>;
  return (
    <div className={classes.pageWrapper}>
      <h3 className={classes.pageTitle}>Доступные курсы</h3>
      {coursesList === undefined ? (
        <CustomBeatLoader />
      ) : coursesList.length > 0 ? (
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
