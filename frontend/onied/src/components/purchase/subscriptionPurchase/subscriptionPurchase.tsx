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
import NotFound from "../../general/responses/notFound/notFound";

function SubscriptionPurchase() {
  const navigate = useNavigate();

  const { subscriptionId } = useParams();
  const [loading, setLoading] = useState<boolean>();
  const [subscription, setSubscription] = useState<PurchaseInfoData | null>();
  const [card, setCard] = useState<CardInfo | null>();
  const [error, setError] = useState<string | null>();

  useEffect(() => {
    api
      .get("/purchases/new/subscription?subscriptionId=" + subscriptionId)
      .then((response: any) => {
        setSubscription(response.data as PurchaseInfoData);
      })
      .catch((error) => {
        if (error.response.status == 404) setSubscription(null);
      });
  }, []);

  if (loading) return <BeatLoader color="var(--accent-color)" />;
  if (subscription == null) return <NotFound>Подписка не найдена</NotFound>;

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
          const purchase = {
            purchaseType: PurchaseType.Subscription,
            price: subscription!.price,
            cardInfo: card!,
            subscriptionId: subscriptionId,
          };
          setLoading(true);
          api
            .post("/purchases/new/subscription", purchase)
            .then(() => navigate(-1))
            .catch((error) => {
              if (error.response.status == 400)
                setError("Возникла ошибка при валидации");
              else if (error.response.status == 403)
                setError("Вы не можете купить данный курс");
              else if (error.response.status >= 500)
                setError("Возникла ошибка на сервере");
              setLoading(false);
            });
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
