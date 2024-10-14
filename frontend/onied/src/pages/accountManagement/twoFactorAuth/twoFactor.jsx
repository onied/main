import classes from "./twoFactor.module.css";
import CardTwoFactor from "../../../components/accountManagement/twoFactorAuth/cardTwoFactor/cardTwoFactor.tsx";
function TwoFactor() {
  return (
    <>
      <div className={classes.container}>
        <div>
          <CardTwoFactor />
        </div>
      </div>
    </>
  );
}

export default TwoFactor;
