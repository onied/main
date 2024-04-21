import { useProfile } from "../../hooks/profile/useProfile";
import { useEffect, useState } from "react";
import PurchaseContainer, { Purchase } from "./purchase/purchase";
import classes from "./profile.module.css";
import Columns from "./purchase/columns";
import { PurchaseType } from "../../types/purchase";
import api from "../../config/axios";

function ProfilePurchases() {
  const [profile, _] = useProfile();
  const [purchasesList, setPurchasesList] = useState<
    Array<Purchase> | undefined
  >();

  useEffect(() => {
    //setPurchasesList(undefined);
    /*api
      .get("purchases")
      .then((res) => {
        console.log(res.data);
        setPurchasesList(res.data);
      })
      .catch((error) => console.log(error));*/
    setPurchasesList([
      {
        id: 1,
        purchaseDetails: {
          purchaseType: PurchaseType.Subscription,
          subscription: { id: 2, title: "Базовая" },
          startDate: new Date("2024-04-17"),
        },
        price: 2000,
      },
      {
        id: 2,
        purchaseDetails: {
          purchaseType: PurchaseType.Subscription,
          subscription: { id: 1, title: "Полная" },
          startDate: new Date("2024-04-17"),
        },
        price: 10000,
      },
      {
        id: 3,
        purchaseDetails: {
          purchaseType: PurchaseType.Course,
          course: { id: 1, title: "Название курса" },
          startDate: new Date("2024-04-17"),
        },
        price: 2000,
      },
      {
        id: 4,
        purchaseDetails: {
          purchaseType: PurchaseType.Course,
          course: {
            id: 2,
            title: "Курс с мегаультрасупердуперпупердлинным названием",
          },
          startDate: new Date("2024-04-17"),
        },
        price: 2000,
      },
    ]);
  }, []);
  if (profile == null) return <></>;

  if (!purchasesList || purchasesList?.length == 0)
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
