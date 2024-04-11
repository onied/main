import paymentMethodsLogo from "../../../assets/paymentMethods.png";
import classes from "./paymentMethods.module.css";

export function PaymentMethodsLogo() {
  return (
    <div className={classes.logoContaier}>
      <img src={paymentMethodsLogo} alt="payment methods logo" />
    </div>
  );
}
