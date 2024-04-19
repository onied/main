import { useProfile } from "../../hooks/profile/useProfile";
import { useEffect, useState } from "react";
import PurchaseContainer, { Purchase } from "./purchase/purchase";
import classes from "./profile.module.css";
import Columns from "./purchase/columns";

function ProfilePurchases() {
  const [profile, _] = useProfile();
  const [purchasesList, setPurchasesList] = useState<
    Array<Purchase> | undefined
  >();

  useEffect(() => {
    setPurchasesList(undefined);
    /*setPurchasesList([
      {
        purchaseDate: new Date("2024-04-17"),
        purchaseType: 3,
        courseId: null,
        name: "Базовая",
        price: 2000,
      },
      {
        purchaseDate: new Date("2024-04-17"),
        purchaseType: 3,
        courseId: null,
        name: "Полная",
        price: 10000,
      },
      {
        purchaseDate: new Date("2024-04-17"),
        purchaseType: 1,
        courseId: 1,
        name: "Название курса",
        price: 2000,
      },
      {
        purchaseDate: new Date("2024-04-17"),
        purchaseType: 2,
        courseId: 2,
        name: "Курс с мегаультрасупердуперпупердлинным названием",
        price: 2000,
      },
    ]);*/
  }, []);
  if (profile == null) return <></>;

  if (!purchasesList)
    return (
      <p className={classes.noCourses} style={{ margin: "40px" }}>
        Вы не совершали покупок
      </p>
    );

  return (
    <div style={{ margin: "0 40px 40px 40px" }}>
      <Columns></Columns>
      {purchasesList.map((purchase, index) => (
        <PurchaseContainer purchase={purchase} key={index}></PurchaseContainer>
      ))}
    </div>
  );
}

export default ProfilePurchases;
