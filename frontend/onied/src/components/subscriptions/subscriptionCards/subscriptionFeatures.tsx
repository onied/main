import classes from "./subscriptionCards.module.css";

function SubscriptionFeatures(props: { features: Array<string> }) {
  return (
    <div className={classes.subscriptionFeaturesList}>
      <ul>
        {props.features.map((feature, index) => (
          <li key={index}>
            <span>{feature}</span>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default SubscriptionFeatures;
