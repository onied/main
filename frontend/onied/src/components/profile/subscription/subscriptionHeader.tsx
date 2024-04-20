import classes from "./subscription.module.css";
import { Subscription } from "./subscription";

function SubscriptionHeader(props: { subscription: Subscription }) {
  const today = new Date();
  const daysLeft = Math.ceil(
    (props.subscription.endDate.getTime() - today.getTime()) /
      (1000 * 60 * 60 * 24)
  );

  const getDaysString = (days: number) => {
    if (days % 10 === 1 && days % 100 != 11) {
      return "день";
    } else if (
      days % 10 > 1 &&
      days % 10 < 5 &&
      (days % 100 < 11 || days % 100 > 15)
    ) {
      return "дня";
    } else {
      return "дней";
    }
  };

  const durationText = `Закончится через ${daysLeft} ${getDaysString(daysLeft)}`;

  return (
    <div className={classes.subscriptionCardHeader}>
      <div
        className={classes.subscriptionTitle}
        style={
          props.subscription.coursesHighlightingEnabled
            ? { fontStyle: "italic" }
            : undefined
        }
      >
        <h1>{props.subscription.title}</h1>
      </div>
      <div style={{ fontSize: "24px" }}>{durationText}</div>
    </div>
  );
}

export default SubscriptionHeader;
