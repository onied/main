import { useProfile } from "../../hooks/profile/useProfile";
import { useEffect, useState } from "react";
import PurchaseContainer, { Purchase } from "./purchase/purchase";
import classes from "./profile.module.css";
import Columns from "./purchase/columns";
import api from "../../config/axios";

function ProfilePurchases() {
  const [profile, _] = useProfile();
  const [purchasesList, setPurchasesList] = useState<
    Array<Purchase> | undefined
  >();

  useEffect(() => {
    api
      .get("purchases")
      .then((res) => {
        setPurchasesList(
          res.data.sort((a: Purchase, b: Purchase) =>
            a.purchaseDetails.purchaseDate > b.purchaseDetails.purchaseDate
              ? -1
              : 1
          )
        );
      })
      .catch((error) => console.log(error));
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
