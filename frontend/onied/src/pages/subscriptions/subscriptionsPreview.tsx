import DefaultSubscription from "../../components/subscriptions/subscriptionCards/defaultSubscription";
import FullSubscription from "../../components/subscriptions/subscriptionCards/fullSubscription";
import classes from "./subscriptionsPreview.module.css";
import { useEffect, useState } from "react";
import BeatLoader from "react-spinners/BeatLoader";
import ElaborateFeaturesList from "../../components/subscriptions/elaborateFeaturesList";

function SubscriptionsPreview() {
  const [defaultSubscriptionInfo, setDefaultSubscriptionInfo] = useState<
    SubscriptionInfo | undefined
  >();
  const [fullSubscriptionInfo, setFullSubscriptionInfo] = useState<
    SubscriptionInfo | undefined
  >();
  const [elaborateFeatureDescriptions, setElaborateFeatureDescriptions] =
    useState<Array<SubscriptionFeatureInfo> | undefined>();

  useEffect(() => {
    setTimeout(() => {
      setDefaultSubscriptionInfo({
        subscriptionId: 1,
        price: 2000,
        durationPolicy: "на одного пользователя в месяц",
        features: ["3 активных платных курса"],
      });
      setFullSubscriptionInfo({
        subscriptionId: 2,
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
      setElaborateFeatureDescriptions([
        {
          featureDescription: "Количество активных платных курсов",
          free: "1",
          default: "3",
          full: "3",
        },
        {
          featureDescription: "Автоматическая проверка тестовых заданий",
          free: true,
          default: true,
          full: true,
        },
        {
          featureDescription: "Неограниченное число учащихся",
          free: true,
          default: true,
          full: true,
        },
        {
          featureDescription: "Реклама в рассылке пользователей",
          free: false,
          default: false,
          full: true,
        },
        {
          featureDescription:
            "Выдача сертификатов пользователю по окончанию курса",
          free: false,
          default: false,
          full: true,
        },
        {
          featureDescription: "Визуальное выделение курсов",
          free: false,
          default: false,
          full: true,
        },
        {
          featureDescription: "Показ на главной странице ",
          free: false,
          default: false,
          full: true,
        },
      ]);
    }, 500);
  }, []);

  if (
    defaultSubscriptionInfo == undefined ||
    fullSubscriptionInfo == undefined ||
    elaborateFeatureDescriptions == undefined
  )
    return (
      <BeatLoader
        cssOverride={{ margin: "30px 30px" }}
        color="var(--accent-color)"
      ></BeatLoader>
    );

  return (
    <div className={classes.pageWrapper}>
      <h1>Выберите вариант вашей подписки</h1>
      <div className={classes.contentWrapper}>
        <div className={classes.subscriptionCardsWrapper}>
          <DefaultSubscription subscriptionInfo={defaultSubscriptionInfo!} />
          <FullSubscription subscriptionInfo={fullSubscriptionInfo!} />
        </div>
        <ElaborateFeaturesList
          featureDescriptions={elaborateFeatureDescriptions}
        />
      </div>
    </div>
  );
}

export default SubscriptionsPreview;

export type SubscriptionInfo = {
  subscriptionId: number;
  price: number;
  durationPolicy: string;
  features: Array<string>;
};

export type SubscriptionFeatureInfo = {
  featureDescription: string;
  free: string | boolean;
  default: string | boolean;
  full: string | boolean;
};
