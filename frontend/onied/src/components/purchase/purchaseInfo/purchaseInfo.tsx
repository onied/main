import { useState } from "react";
import { PurchaseType } from "../../../types/purchases";

import classes from "./purchaseInfo.module.css";

function PurchaseInfo() {
  const PurchaseTypeDescription = {
    [PurchaseType.Course]: "Курс",
    [PurchaseType.Certificate]: "Сертификат",
    [PurchaseType.Subscription]: "Подписка",
  };
  const [title, setTitle] = useState<string>("Для серьезных людей");
  const [purchaseType, setPurchaseType] = useState<PurchaseType>(
    PurchaseType.Subscription
  );
  const [price, setPrice] = useState<number>(100_500);

  return (
    <div className={classes.purchaseInfoContainer}>
      <div className={classes.title}>{title}</div>
      <div className={classes.additionalInfo}>
        <span className={classes.purchaseType}>
          {PurchaseTypeDescription[purchaseType]}
        </span>
        <div className={classes.purchasePrice}>{price} ₽</div>
      </div>
    </div>
  );
}

export default PurchaseInfo;
