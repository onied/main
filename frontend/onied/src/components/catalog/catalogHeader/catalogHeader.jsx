import classes from "./catalogHeader.module.css";

function CatalogHeader() {
  return (
    <div className={classes.catalogHeader}>
      <h2>Онлайн курсы</h2>
      <div className={classes.sort}>
        <label className={classes.label}>Сортировка: </label>
        <span>лучшее совпадение</span>
      </div>
    </div>
  );
}

export default CatalogHeader;
