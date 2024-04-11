import { SubscriptionInfo } from "../../pages/subscriptions/subscriptionsPreview";
import SubscriptionHeader from "./subscriptionHeader";
import classes from "./subscriptions.module.css";
import SubscriptionFeatures from "./subscriptionFeatures";

function DefaultSubscription(props: { subscriptionInfo: SubscriptionInfo }) {
  return (
    <div className={classes.defaultSubscriptionCard}>
      <SubscriptionHeader
        subscriptionType={"Базовый"}
        price={props.subscriptionInfo.price}
        durationPolicy={props.subscriptionInfo.durationPolicy}
      />
      <SubscriptionFeatures features={props.subscriptionInfo.features} />
    </div>
  );
}

export default DefaultSubscription;
