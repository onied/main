import CourseCardsContainer from "../../components/catalog/courseCardsContainer.jsx";
import CatalogHeader from "../../components/catalog/catalogHeader/catalogHeader.jsx";
import CatalogNavigation from "../../components/catalog/catalogNavigation/catalogNavigation.jsx";
import { useEffect, useState } from "react";
import { getAmountOfPages } from "../../components/catalog/catalogPages.js";

function Catalog() {
  const [currentPage, setCurrentPage] = useState(1);
  const [pagesCount, setPagesCount] = useState(1);
  const [coursesList, setCoursesList] = useState();

  return (
    <div>
      <CatalogHeader />
      <CourseCardsContainer coursesList={coursesList} />
      <CatalogNavigation
        currentPage={currentPage}
        pagesCount={pagesCount}
        setCurrentPage={setCurrentPage}
      />
    </div>
  );
}

export default Catalog;
