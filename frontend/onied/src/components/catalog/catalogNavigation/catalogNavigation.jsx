import classes from "./catalogNavigation.module.css";
import NaviationPageButton from "./naviationPageButton.jsx";

function CatalogNavigation({ currentPage, setCurrentPage, pagesCount }) {
  const pageNumbers = Array.from({ length: pagesCount }, (_, i) => i + 1);

  return (
    <div className={classes.catalogNavigationContainer}>
      <div
        className={classes.navItem}
        onClick={() => setCurrentPage(currentPage - 1)}>
        {"<"}
      </div>
      <div
        className={classes.navItem}
        onClick={() => setCurrentPage(currentPage - 1)}>
        Назад
      </div>
      {currentPage - 2 > 1 ? (
        <div style={{ padding: "0 15px" }}>...</div>
      ) : null}
      {pageNumbers.map((pageNumber) => {
        if (pageNumber <= currentPage + 2 && pageNumber >= currentPage - 2) {
          return (
            <NaviationPageButton
              key={pageNumber}
              pageNumber={pageNumber}
              isActive={pageNumber === currentPage}
              setCurrentPage={setCurrentPage}
            />
          );
        }
      })}
      {currentPage + 2 < pagesCount ? (
        <div style={{ padding: "0 15px" }}>...</div>
      ) : null}
      <div
        className={classes.navItem}
        onClick={() => setCurrentPage(currentPage + 1)}>
        Вперёд
      </div>
      <div
        className={classes.navItem}
        onClick={() => setCurrentPage(currentPage + 1)}>
        {">"}
      </div>
    </div>
  );
}

export default CatalogNavigation;
