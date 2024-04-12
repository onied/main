import classes from "./subscriptionCards.module.css";

function SubscriptionHeader(props: {
  subscriptionType: string;
  price: number;
  durationPolicy: string;
}) {
  const slantStyle =
    props.subscriptionType.toLowerCase() == "полный"
      ? { fontStyle: "italic" }
      : undefined;

  const getPriceWithSpaces = (price: number): string => {
    return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
  };

  return (
    <div className={classes.subscriptionCardHeader}>
      <div className={classes.subscriptionTitle} style={slantStyle}>
        <h1>{props.subscriptionType}</h1>
      </div>
      <div className={classes.subscriptionPrice}>
        <h1>{getPriceWithSpaces(props.price)} ₽</h1>
      </div>
      <div>{props.durationPolicy}</div>
    </div>
  );
}

export default SubscriptionHeader;
