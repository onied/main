import classes from "./teaching.module.css";
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

function TeachingAuthored() {
  const [profile, _] = useProfile();
  const [coursesList, setCoursesList] = useState<
    Array<CourseCard> | undefined
  >();
  useEffect(() => {
    if (profile == null) return;
    api
      .get("/teaching/authored")
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
      {coursesList === undefined ? (
        <CustomBeatLoader />
      ) : coursesList.length > 0 ? (
        <CourseCardsContainer coursesList={coursesList}></CourseCardsContainer>
      ) : (
        <p className={classes.noCourses}>Нет созданных курсов</p>
      )}
    </div>
  );
}

export default TeachingAuthored;
