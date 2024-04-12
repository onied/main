import Button from "../../general/button/button";
import CardContainer from "../cardContainer";
import PurchaseInfo from "../purchaseInfo/purchaseInfo";

import classes from "./coursePurchase.module.css";

function CoursePurchase() {
  return (
    <div className={classes.coursePurchaseContainer}>
      <h2 className={classes.pageTitle}>Покупка</h2>
      <PurchaseInfo />
      <CardContainer />
      <div className={classes.purchaseFooter}>
        <Button>оплатить</Button>
        <Button>отмена</Button>
      </div>
    </div>
  );
}

export default CoursePurchase;
