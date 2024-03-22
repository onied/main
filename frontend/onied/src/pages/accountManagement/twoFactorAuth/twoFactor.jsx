import classes from "./twoFactor.module.css";
import CardTwoFactor from "../../../components/accountManagement/twoFactorAuth/cardTwoFactor/cardTwoFactor.jsx";
function TwoFactor() {
  return (
    <>
      <div className={classes.container}>
        <div>
          <CardTwoFactor></CardTwoFactor>
        </div>
      </div>
    </>
  );
}

export default TwoFactor;
