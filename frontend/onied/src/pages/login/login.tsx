import { ReactNode } from "react";

import classes from "./login.module.css";
import SignInLogo from "../../assets/signIn.svg";
import SignInForm from "../../components/login/loginForm/loginForm";
import RegistrationForm from "../../components/login/registrationForm/registrationForm";

function Login(): ReactNode {
  return (
    <div className={classes.contentContainer}>
      <div className={classes.loginImg}>
        <img src={SignInLogo} />
      </div>
      <div className={classes.rightBlock}>
        <SignInForm />
        <RegistrationForm />
      </div>
    </div>
  );
}

export default Login;
