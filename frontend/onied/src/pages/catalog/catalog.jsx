import CourseCardsContainer from "../../components/catalog/courseCardsContainer.jsx";
import CatalogHeader from "../../components/catalog/catalogHeader/catalogHeader.jsx";
import CatalogNavigation from "../../components/catalog/catalogNavigation/catalogNavigation.jsx";
import { useEffect, useState } from "react";
import api from "../../config/axios.ts";
import classes from "./catalog.module.css";
import CatalogFilter from "../../components/catalog/catalogFilter/catalogFilter.tsx";

function Catalog() {
  const [currentPage, setCurrentPage] = useState(1);
  const [pagesCount, setPagesCount] = useState(1);
  const [coursesList, setCoursesList] = useState();

  useEffect(() => {
    setCoursesList(undefined);
    api
      .get("catalog/?page=" + currentPage)
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
      });
  }, [currentPage]);

  return (
    <div className={classes.container}>
      <div></div>
      <CatalogHeader />
      <CatalogFilter />
      <div>
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
