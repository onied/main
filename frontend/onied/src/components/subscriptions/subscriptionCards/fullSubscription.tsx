import { SubscriptionInfo } from "../../../pages/subscriptions/subscriptionsPreview";
import classes from "./subscriptionCards.module.css";
import SubscriptionHeader from "./subscriptionHeader";
import SubscriptionFeatures from "./subscriptionFeatures";
import { Link } from "react-router-dom";
import Button from "../../general/button/button";

function FullSubscription(props: { subscriptionInfo: SubscriptionInfo }) {
  return (
    <div className={classes.fullSubscriptionCard}>
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

export default FullSubscription;
