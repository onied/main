import { SubscriptionInfo } from "../../pages/subscriptions/subscriptionsPreview";
import classes from "./subscriptions.module.css";
import SubscriptionHeader from "./subscriptionHeader";
import SubscriptionFeatures from "./subscriptionFeatures";

function FullSubscription(props: { subscriptionInfo: SubscriptionInfo }) {
  return (
    <div className={classes.fullSubscriptionCard}>
      <SubscriptionHeader
        subscriptionType={"Полный"}
        price={props.subscriptionInfo.price}
        durationPolicy={props.subscriptionInfo.durationPolicy}
      />
      <SubscriptionFeatures features={props.subscriptionInfo.features} />
    </div>
  );
}

export default FullSubscription;
