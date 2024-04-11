import classes from "./subscriptions.module.css";

function SubscriptionHeader(props: {
  subscriptionType: string;
  price: number;
  durationPolicy: string;
}) {
  return (
    <div className={classes.subscriptionCardHeader}>
      <div>
        <h1>{props.subscriptionType}</h1>
      </div>
      <div>
        <h1>{props.price} ₽</h1>
      </div>
      <div>{props.durationPolicy}</div>
    </div>
  );
}

export default SubscriptionHeader;
