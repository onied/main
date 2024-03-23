import classes from "./forgotPassword.module.css";
import ForgotPasswordComponent from "../../components/forgotPassword/forgotPassword.js";

function ForgotPassword() {
  return (
    <div className={classes.container}>
      <ForgotPasswordComponent></ForgotPasswordComponent>
    </div>
  );
}

export default ForgotPassword;
