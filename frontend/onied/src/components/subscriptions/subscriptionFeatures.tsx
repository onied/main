import classes from "./subscriptions.module.css";

function SubscriptionFeatures(props: { features: Array<string> }) {
  return (
    <div className={classes.subscriptionFeaturesList}>
      {props.features.map((feature) => (
        <ul>
          <li>
            <span>{feature}</span>
          </li>
        </ul>
      ))}
    </div>
  );
}

export default SubscriptionFeatures;
