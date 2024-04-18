import InputForm from "../../general/inputform/inputform";
import classes from "./catalogFilter.module.css";

function CatalogFilter() {
  return (
    <div className={classes.container}>
      <div className={classes.filterContainer}>
        <h3 className={classes.title}>Фильтрация</h3>
        <label className={classes.label}>Категория</label>
        <InputForm placeholder="Любая категория"></InputForm>
        <label className={classes.label}>Цена</label>
        <div className={classes.interval}>
          <InputForm
            placeholder="От"
            type="number"
            className={classes.intervalInput}
          ></InputForm>
          <span>—</span>
          <InputForm
            placeholder="До"
            type="number"
            className={classes.intervalInput}
          ></InputForm>
          <span className={classes.intervalLabel}>₽</span>
        </div>
        <label className={classes.label}>Время прохождения</label>
        <div className={classes.interval}>
          <InputForm
            placeholder="От"
            type="number"
            className={classes.intervalInput}
          ></InputForm>
          <span className={classes.label}>—</span>
          <InputForm
            placeholder="До"
            type="number"
            className={classes.intervalInput}
          ></InputForm>
          <span className={classes.intervalLabel}>ч.</span>
        </div>
        <div className={classes.checkboxContainer}>
          <label className={classes.label}>Только с сертификатом</label>
          <input type="checkbox"></input>
        </div>
        <div className={classes.checkboxContainer}>
          <label className={classes.label}>Только активные</label>
          <input type="checkbox"></input>
        </div>
      </div>
    </div>
  );
}

export default CatalogFilter;
