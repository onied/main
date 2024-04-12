import { useNavigate, useParams } from "react-router-dom";
import Button from "../../general/button/button";
import CardContainer from "../cardContainer";
import PurchaseInfo from "../purchaseInfo/purchaseInfo";

import classes from "./subscriptionPurchase.module.css";
import {
  CardInfo,
  PurchaseInfoData,
  PurchaseType,
} from "../../../types/purchase";
import { useEffect, useState } from "react";
import api from "../../../config/axios";
import { BeatLoader } from "react-spinners";

function SubscriptionPurchase() {
  const NotFound = <h2>Курс не найден</h2>;

  const navigate = useNavigate();

  const { subscriptionId } = useParams();
  const [subscription, setSubscription] = useState<
    PurchaseInfoData | null | undefined
  >(undefined);
  const [card, setCard] = useState<CardInfo | null>();
  const [error, setError] = useState<string | null>();

  useEffect(() => {
    // api
    //   .get("/subscriptions/" + courseId)
    //   .then((response: any) => {
    //     setCourse(response.data as CoursePurchaseInfo);
    //   })
    //   .catch((error) => {
    //     if (error.response.status == 404) setCourse(null);
    //   });
    const stub: PurchaseInfoData = {
      id: 1,
      title: "для серьезных людей",
      price: 100_500,
    };
    setSubscription(stub);
  }, []);

  if (subscription === undefined) return <BeatLoader />;
  if (subscription === null) return NotFound;

  return (
    <div className={classes.subscriptionPurchaseContainer}>
      <h2 className={classes.pageTitle}>Покупка</h2>
      <PurchaseInfo
        title={subscription.title}
        price={subscription.price}
        purchaseType={PurchaseType.Subscription}
      />
      {error != null && <div className={classes.error}>{error}</div>}
      <form
        style={{
          display: "flex",
          flexDirection: "column",
          gap: "0.5rem",
          alignItems: "center",
        }}
        onSubmit={(event) => {
          event.preventDefault();
          // логика
        }}
      >
        <CardContainer onChange={setCard} />
        <div className={classes.purchaseFooter}>
          {card == null ? (
            <Button disabled>оплатить</Button>
          ) : (
            <Button>оплатить</Button>
          )}
          <Button
            onClick={(event: any) => {
              event.preventDefault();
              navigate(-1);
            }}
          >
            отмена
          </Button>
        </div>
      </form>
    </div>
  );
}

export default SubscriptionPurchase;
