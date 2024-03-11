import classes from "./catalogNavigation.module.css";

function NavigationPageButton({ pageNumber, isActive, setCurrentPage }) {
  return (
    <div>
      <div
        className={[
          classes.navItem,
          isActive ? classes.activeNavItem : "",
        ].join(" ")}
        onClick={() => setCurrentPage(pageNumber)}>
        {pageNumber}
      </div>
    </div>
  );
}

export default NavigationPageButton;
