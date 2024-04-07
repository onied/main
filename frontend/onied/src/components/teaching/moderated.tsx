import classes from "./teaching.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import CourseCardsContainer from "../catalog/courseCardsContainer";
import { useEffect, useState } from "react";
import api from "../../config/axios";
import { BeatLoader } from "react-spinners";

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

function TeachingModerated() {
  const [profile, _] = useProfile();
  const [coursesList, setCoursesList] = useState<
    Array<CourseCard> | undefined
  >();
  useEffect(() => {
    if (profile == null) return;
    api
      .get("/teaching/moderated")
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
        <BeatLoader color="var(--accent-color)"></BeatLoader>
      ) : coursesList.length > 0 ? (
        <CourseCardsContainer
          coursesList={coursesList}
          owned={true}
        ></CourseCardsContainer>
      ) : (
        <p className={classes.noCourses}>Нет модерируемых курсов</p>
      )}
    </div>
  );
}

export default TeachingModerated;
