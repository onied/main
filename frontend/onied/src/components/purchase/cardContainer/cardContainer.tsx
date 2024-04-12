import InputForm from "../../general/inputform/inputform";
import PaymentMethodsLogo from "../paymentMethods";
import classes from "./cardContainer.module.css";

function CardContainer() {
  return (
    <div className={classes.cardContainer}>
      <PaymentMethodsLogo className={classes.paymentMethods} />
      <InputForm placeholder="Номер карты" style={{ width: "100%" }} />
      <InputForm placeholder="Держатель карты" style={{ width: "100%" }} />
      <div className={classes.cardFooter}>
        <div className={classes.monthAndYear}>
          <InputForm placeholder="мм" />
          <span>/</span>
          <InputForm placeholder="гг" />
        </div>
        <div className={classes.securityCode}>
          <InputForm placeholder="CVC/CVV/CVP" />
          <span style={{ textWrap: "wrap" }}>
            три цифры с оборотной стороны
          </span>
        </div>
      </div>
    </div>
  );
}

export default CardContainer;
