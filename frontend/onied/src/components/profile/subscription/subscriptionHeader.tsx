import classes from "./subscription.module.css";
import { SubscriptionType } from "../../../pages/subscriptions/subscriptionsPreview";

function SubscriptionHeader(props: {
  subscriptionType: SubscriptionType;
  endDate: Date;
}) {
  const subscriptionTypeMapByStyle = {
    0: undefined,
    1: { fontStyle: "italic" },
  };
  const slantStyle = subscriptionTypeMapByStyle[props.subscriptionType];

  const subscriptionTypeMapByName = {
    0: "Базовая",
    1: "Полная",
  };
  const name = subscriptionTypeMapByName[props.subscriptionType];

  const today = new Date();
  const daysLeft = Math.ceil(
    (props.endDate.getTime() - today.getTime()) / (1000 * 60 * 60 * 24)
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
      <div className={classes.subscriptionTitle} style={slantStyle}>
        <h1>{name}</h1>
      </div>
      <div style={{ fontSize: "24px" }}>{durationText}</div>
    </div>
  );
}

export default SubscriptionHeader;
