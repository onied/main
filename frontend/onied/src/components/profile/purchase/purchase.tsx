import classes from "./purchase.module.css";
import { format } from "date-fns";
import ruLocale from "date-fns/locale/ru";
import { Link } from "react-router-dom";
import { PurchaseType } from "../../../types/purchase";

export type Course = {
  id: number;
  title: string;
};

export type Subscription = {
  id: number;
  title: string;
};

export type PurchaseDetails = {
  purchaseType: PurchaseType;
  course?: Course;
  subscription?: Subscription;
  purchaseDate: Date;
};

export type Purchase = {
  id: number;
  purchaseDetails: PurchaseDetails;
  price: number;
};

function PurchaseContainer({ purchase }: { purchase: Purchase }) {
  const formatDate = (date: Date) => {
    return format(new Date(date), "d MMMM yyyy", { locale: ruLocale });
  };

  const purchaseTypeMap = {
    [PurchaseType.Any]: "",
    [PurchaseType.Course]: "Курс",
    [PurchaseType.Certificate]: "Сертификат",
    [PurchaseType.Subscription]: "Подписка",
  };

  return (
    <div className={classes.rowPurchase}>
      <div className={classes.leftColumns}>
        <p style={{ width: "168pt" }}>
          {formatDate(purchase.purchaseDetails.purchaseDate)}
        </p>
        <p style={{ width: "152pt" }}>
          {purchaseTypeMap[purchase.purchaseDetails.purchaseType]}
        </p>
        {purchase.purchaseDetails.course ? (
          <Link
            to={`/course/${purchase.purchaseDetails.course.id}`}
            className={classes.name}
          >
            {purchase.purchaseDetails.course.title}
          </Link>
        ) : (
          <p className={classes.name}>
            {purchase.purchaseDetails.subscription?.title}
          </p>
        )}
      </div>
      <div className={classes.rightColumns}>
        <p>{purchase.price} ₽</p>
      </div>
    </div>
  );
}

export default PurchaseContainer;
