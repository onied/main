import classes from "./purchase.module.css";

function Columns() {
  return (
    <div className={classes.rowPurchase + " " + classes.columns}>
      <div className={classes.leftColumns}>
        <h3 className={classes.nameColumn} style={{ width: "168pt" }}>
          Дата покупки
        </h3>
        <h3 className={classes.nameColumn} style={{ width: "152pt" }}>
          Тип покупки
        </h3>
        <h3 className={classes.nameColumn}>Название</h3>
      </div>
      <div className={classes.rightColumns}>
        <h3 className={classes.nameColumn}>Стоимость</h3>
      </div>
    </div>
  );
}

export default Columns;
