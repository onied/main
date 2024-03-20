import { ReactNode } from "react";

import classes from "./login.module.css";
import SignInLogo from "../../assets/signIn.svg";
import SignInForm from "../../components/login/loginForm/loginForm";
import RegistrationForm from "../../components/login/registrationForm/registrationForm";

function Login(): ReactNode {
  return (
    <>
      <img className={classes.loginImg} src={SignInLogo} />
      <div className={classes.rightBlock}>
        <SignInForm />
        <RegistrationForm />
      </div>
    </>
  );
}

export default Login;
