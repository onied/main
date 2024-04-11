import InputForm from "../../general/inputform/inputform";
import PaymentMethodsLogo from "../paymentMethods";
import classes from "./cardContainer.module.css";

function CardContainer() {
  return (
    <div className={classes.cardContainer}>
      <PaymentMethodsLogo />
      <div className={classes.monthAndYear}>
        <InputForm placeholder="мм" style={{ width: "1.25rem" }} />
        <span>/</span>
        <InputForm placeholder="гг" style={{ width: "1.25rem" }} />
      </div>
    </div>
  );
}

export default CardContainer;
