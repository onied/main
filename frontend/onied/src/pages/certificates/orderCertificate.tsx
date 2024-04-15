import OrderCertificate from "../../components/certificates/orderCertificate/orderCertificate";
import classes from "./orderCertificate.module.css";

function OrderCertificatePage() {
  return (
    <div className={classes.container}>
      <div className={classes.innerContainer}>
        <OrderCertificate />
      </div>
    </div>
  );
}

export default OrderCertificatePage;
