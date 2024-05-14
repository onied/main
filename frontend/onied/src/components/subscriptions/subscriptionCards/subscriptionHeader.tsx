import classes from "./subscriptionCards.module.css";
import { SubscriptionType } from "../../../pages/subscriptions/subscriptionsPreview";

function SubscriptionHeader(props: {
  isHighlighted: boolean;
  title: string;
  price: number;
  durationPolicy: string;
}) {
  const slantStyle = props.isHighlighted ? { fontStyle: "italic" } : undefined;

  const getPriceWithSpaces = (price: number): string => {
    return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
  };

  return (
    <div className={classes.subscriptionCardHeader}>
      <div className={classes.subscriptionTitle} style={slantStyle}>
        <h1>{props.title}</h1>
      </div>
      <div className={classes.subscriptionPrice}>
        <h1>{getPriceWithSpaces(props.price)} ₽</h1>
      </div>
      <div>{props.durationPolicy}</div>
    </div>
  );
}

export default SubscriptionHeader;
