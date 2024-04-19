import classes from "./purchase.module.css";
import { format } from "date-fns";
import ruLocale from "date-fns/locale/ru";
import { Link } from "react-router-dom";

export enum PurchaseType {
  Any,
  Course,
  Certificate,
  Subscription,
}

export type Purchase = {
  purchaseDate: Date;
  purchaseType: PurchaseType;
  courseId: number | null;
  name: string;
  price: number;
};

function PurchaseContainer({ purchase }: { purchase: Purchase }) {
  const formatDate = (date: Date) => {
    return format(new Date(date), "d MMMM yyyy", { locale: ruLocale });
  };

  const purchaseTypeMap = {
    0: "",
    1: "Курс",
    2: "Сертификат",
    3: "Подписка",
  };

  return (
    <div className={classes.rowPurchase}>
      <div className={classes.leftColumns}>
        <p style={{ width: "168pt" }}>{formatDate(purchase.purchaseDate)}</p>
        <p style={{ width: "152pt" }}>
          {purchaseTypeMap[purchase.purchaseType]}
        </p>
        {purchase.courseId ? (
          <Link to={`/course/${purchase.courseId}`} className={classes.name}>
            {purchase.name}
          </Link>
        ) : (
          <p className={classes.name}>{purchase.name}</p>
        )}
      </div>
      <div className={classes.rightColumns}>
        <p>{purchase.price} ₽</p>
      </div>
    </div>
  );
}

export default PurchaseContainer;
