import { ReactNode } from "react";

import classes from "./login.module.css";
import SignInLogo from "../../../assets/signIn.svg";
import SignInForm from "../../../components/accountManagement/login/loginForm/loginForm";
import RegistrationForm from "../../../components/accountManagement/login/registrationForm/registrationForm";

function Login(): ReactNode {
  return (
    <div className={classes.mainContainer}>
      <img className={classes.loginImg} src={SignInLogo} />
      <div className={classes.rightBlock}>
        <SignInForm />
        <RegistrationForm />
      </div>
    </div>
  );
}

export default Login;
