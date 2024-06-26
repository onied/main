import { SubscriptionInfo } from "../../../pages/subscriptions/subscriptionsPreview";
import SubscriptionHeader from "./subscriptionHeader";
import classes from "./subscriptionCards.module.css";
import SubscriptionFeatures from "./subscriptionFeatures";
import Button from "../../general/button/button";
import { Link } from "react-router-dom";

function DefaultSubscription(props: { subscriptionInfo: SubscriptionInfo }) {
  return (
    <div className={classes.defaultSubscriptionCard}>
      <SubscriptionHeader
        isHighlighted={props.subscriptionInfo.isHighlighted}
        title={props.subscriptionInfo.title}
        price={props.subscriptionInfo.price}
        durationPolicy={props.subscriptionInfo.durationPolicy}
      />
      <SubscriptionFeatures features={props.subscriptionInfo.features} />
      <Link
        to={`/purchases/subscription/${props.subscriptionInfo.subscriptionId}`}
        className={classes.buySubscriptionButton}
      >
        <Button>оформить</Button>
      </Link>
    </div>
  );
}

export default DefaultSubscription;
