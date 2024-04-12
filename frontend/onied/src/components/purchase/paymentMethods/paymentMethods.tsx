import paymentMethodsLogo from "../../../assets/paymentMethods.png";
import classes from "./paymentMethods.module.css";

export function PaymentMethodsLogo({ className }: { className?: string }) {
  return (
    <div className={[classes.logoContaier, className].join(" ")}>
      <img src={paymentMethodsLogo} alt="payment methods logo" />
    </div>
  );
}
