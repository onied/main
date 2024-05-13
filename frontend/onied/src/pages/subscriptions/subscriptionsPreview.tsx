import DefaultSubscription from "../../components/subscriptions/subscriptionCards/defaultSubscription";
import FullSubscription from "../../components/subscriptions/subscriptionCards/fullSubscription";
import classes from "./subscriptionsPreview.module.css";
import { useEffect, useState } from "react";
import BeatLoader from "react-spinners/BeatLoader";
import ElaborateFeaturesList from "../../components/subscriptions/elaborateFeaturesList";
import api from "../../config/axios";
import {
  mapSubscriptionFeaturesInfo,
  mapSubscriptionInfo,
  SubscriptionInfoDto,
} from "./mapSubscriptionInfo";

function SubscriptionsPreview() {
  const [defaultSubscriptionInfo, setDefaultSubscriptionInfo] = useState<
    SubscriptionInfo | undefined
  >();
  const [fullSubscriptionInfo, setFullSubscriptionInfo] = useState<
    SubscriptionInfo | undefined
  >();
  const [elaborateFeatureDescriptions, setElaborateFeatureDescriptions] =
    useState<Array<SubscriptionFeatureInfo> | undefined>();
  const [subscriptionTitles, setSubscriptionTitles] = useState<
    Array<string> | undefined
  >();

  useEffect(() => {
    api
      .get("purchases/subscriptions/all")
      .then((response) => {
        console.log(response.data);
        let mappedSubscriptions = mapSubscriptionInfo(response.data);
        setDefaultSubscriptionInfo(mappedSubscriptions[1]);
        setFullSubscriptionInfo(mappedSubscriptions[2]);

        let mappedFeaturesInfos = mapSubscriptionFeaturesInfo(response.data);
        setElaborateFeatureDescriptions(mappedFeaturesInfos);
        setSubscriptionTitles(
          response.data.map((subDto: SubscriptionInfoDto) => subDto.title)
        );
      })
      .catch((error) => console.log(error));
  }, []);

  if (
    defaultSubscriptionInfo == undefined ||
    fullSubscriptionInfo == undefined ||
    elaborateFeatureDescriptions == undefined ||
    subscriptionTitles == undefined
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
          subscriptionsTitles={subscriptionTitles!}
        />
      </div>
    </div>
  );
}

export default SubscriptionsPreview;

export type SubscriptionInfo = {
  subscriptionId: number;
  title: string;
  price: number;
  isHighlighted: boolean;
  durationPolicy: string;
  features: Array<string>;
};

export type SubscriptionFeatureInfo = {
  featureDescription: string;
  free: string | boolean;
  default: string | boolean;
  full: string | boolean;
};
