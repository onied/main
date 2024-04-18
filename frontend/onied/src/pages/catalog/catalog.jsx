import CourseCardsContainer from "../../components/catalog/courseCardsContainer.jsx";
import CatalogHeader from "../../components/catalog/catalogHeader/catalogHeader.jsx";
import CatalogNavigation from "../../components/catalog/catalogNavigation/catalogNavigation.jsx";
import { useEffect, useState } from "react";
import api from "../../config/axios.ts";
import classes from "./catalog.module.css";
import NotificationContainer from "../../components/notifications/notificationContainer/notificationContainer";

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
    <div>
      <CatalogHeader />
      <NotificationContainer
        notifications={[
          {
            id: 1,
            title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
            img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
            message: "z fdgfgfdsgfsdfaasd dsfsdfd",
          },
          {
            id: 1,
            title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
            img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
            message: "z fdgfgfdsgfsdfaasd dsfsdfd",
          },
        ]}
      />
      <div className={classes.courseCardsContainerContainer}>
        <CourseCardsContainer coursesList={coursesList} />
      </div>
      <CatalogNavigation
        currentPage={currentPage}
        maxPageAmount={pagesCount}
        onPageChange={(newPage) =>
          newPage >= 1 && newPage <= pagesCount ? setCurrentPage(newPage) : null
        }
      />
    </div>
  );
}

export default Catalog;
