import { PurchaseType } from "../../../types/purchase";

import classes from "./purchaseInfo.module.css";

function PurchaseInfo({
  title,
  price,
  purchaseType,
}: {
  title: string;
  price: number;
  purchaseType: PurchaseType;
}) {
  const PurchaseTypeDescription = {
    [PurchaseType.Course]: "Курс",
    [PurchaseType.Certificate]: "Сертификат",
    [PurchaseType.Subscription]: "Подписка",
    [PurchaseType.Any]: "",
  };

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
