import CourseCardsContainer from "../../components/catalog/courseCardsContainer.jsx";
import CatalogHeader from "../../components/catalog/catalogHeader/catalogHeader.jsx";
import CatalogNavigation from "../../components/catalog/catalogNavigation/catalogNavigation.jsx";
import { useEffect, useRef, useState } from "react";
import api from "../../config/axios.ts";
import classes from "./catalog.module.css";
import CatalogFilter from "../../components/catalog/catalogFilter/catalogFilter.tsx";
import { useSearchParams } from "react-router-dom";

function Catalog() {
  const [searchParams, _] = useSearchParams();
  const [currentPage, setCurrentPage] = useState(1);
  const [pagesCount, setPagesCount] = useState(1);
  const [coursesList, setCoursesList] = useState();
  const loadingCourses = useRef(false);

  useEffect(() => {
    if (loadingCourses.current) return;
    setCoursesList(undefined);
    loadingCourses.current = true;
    console.log(searchParams);
    api
      .get("catalog/?page=" + currentPage, { params: searchParams })
      .then((response) => {
        console.log(response.data);
        setCoursesList(response.data.elements);
        setPagesCount(response.data.pagesCount);
      })
      .catch((error) => {
        console.log(error);
        setCoursesList(undefined);
        setPagesCount(1);
        setCurrentPage(1);
      })
      .finally(() => {
        loadingCourses.current = false;
      });
  }, [currentPage, searchParams]);

  return (
    <div className={classes.container}>
      <div className={classes.upRight}>
        <CatalogHeader />
      </div>
      <div className={classes.downLeft}>
        <CatalogFilter />
      </div>
      <div className={classes.downRight}>
        <div className={classes.courseCardsContainerContainer}>
          <CourseCardsContainer coursesList={coursesList} />
        </div>
        <CatalogNavigation
          currentPage={currentPage}
          maxPageAmount={pagesCount}
          onPageChange={(newPage) =>
            newPage >= 1 && newPage <= pagesCount
              ? setCurrentPage(newPage)
              : null
          }
        />
      </div>
    </div>
  );
}

export default Catalog;
