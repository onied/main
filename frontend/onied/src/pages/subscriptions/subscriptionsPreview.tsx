import DefaultSubscription from "../../components/subscriptions/defaultSubscription";
import FullSubscription from "../../components/subscriptions/fullSubscription";
import classes from "./subscriptionsPreview.module.css";
import { useEffect, useState } from "react";
import BeatLoader from "react-spinners/BeatLoader";

function SubscriptionsPreview() {
  const [defaultSubscriptionInfo, setDefaultSubscriptionInfo] = useState<
    SubscriptionInfo | undefined
  >();
  const [fullSubscriptionInfo, setFullSubscriptionInfo] = useState<
    SubscriptionInfo | undefined
  >();

  useEffect(() => {
    setTimeout(() => {
      setDefaultSubscriptionInfo({
        price: 2000,
        durationPolicy: "на одного пользователя в месяц",
        features: ["3 активных платных курса"],
      });
      setFullSubscriptionInfo({
        price: 10000,
        durationPolicy: "на одного пользователя в месяц",
        features: [
          "Реклама в рассылке",
          "Выдача сертификатов",
          "3 активных платных курса",
          "Показ на главной странице",
          "Визуальное выделение курсов",
        ],
      });
    }, 500);
  }, []);

  if (defaultSubscriptionInfo == undefined || fullSubscriptionInfo == undefined)
    return (
      <BeatLoader
        cssOverride={{ margin: "30px 30px" }}
        color="var(--accent-color)"
      ></BeatLoader>
    );

  return (
    <div className={classes.pageWrapper}>
      <h1>Выберите вариант вашей подписки</h1>
      <div className={classes.subscriptionCardsWrapper}>
        <DefaultSubscription subscriptionInfo={defaultSubscriptionInfo!} />
        <FullSubscription subscriptionInfo={fullSubscriptionInfo!} />
      </div>
    </div>
  );
}

export default SubscriptionsPreview;

export type SubscriptionInfo = {
  price: number;
  durationPolicy: string;
  features: Array<string>;
};
