import classes from "./catalogNavigation.module.css";
import NaviationPageButton from "./naviationPageButton.jsx";

function CatalogNavigation({ currentPage, onPageChange, maxPageAmount }) {
  const pageNumbers = Array.from({ length: maxPageAmount }, (_, i) => i + 1);

  return (
    <div className={classes.catalogNavigationContainer}>
      <div className={classes.navItem} onClick={() => onPageChange(1)}>
        {"<"}
      </div>
      <div
        className={classes.navItem}
        onClick={() => onPageChange(currentPage - 1)}>
        Назад
      </div>
      {currentPage - 2 > 1 ? (
        <div style={{ padding: "0 15px" }}>...</div>
      ) : null}
      {pageNumbers.map((pageNumber) => {
        if (pageNumber <= currentPage + 2 && pageNumber >= currentPage - 2) {
          return (
            <NaviationPageButton
              pageNumber={pageNumber}
              isActive={pageNumber === currentPage}
              onPageChange={onPageChange}
            />
          );
        }
      })}
      {currentPage + 2 < maxPageAmount ? (
        <div style={{ padding: "0 15px" }}>...</div>
      ) : null}
      <div
        className={classes.navItem}
        onClick={() => onPageChange(currentPage + 1)}>
        Вперёд
      </div>
      <div
        className={classes.navItem}
        onClick={() => onPageChange(maxPageAmount)}>
        {">"}
      </div>
    </div>
  );
}

export default CatalogNavigation;
