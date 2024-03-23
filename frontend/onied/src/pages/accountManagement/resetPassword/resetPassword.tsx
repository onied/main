import classes from "./resetPassword.module.css";
import ResetPasswordComponent from "../../../components/accountManagement/resetPassword/resetPassword.js";

function ResetPassword() {
  return (
    <div className={classes.container}>
      <ResetPasswordComponent></ResetPasswordComponent>
    </div>
  );
}

export default ResetPassword;
