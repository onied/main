import { ReactNode, useState } from "react";
import { useNavigate } from "react-router-dom";

import axios from "axios";

import Config from "../../config/config";
import classes from "./signIn.module.css";
import SignInForm from "../../components/signin/signInForm/signInForm";
import SignInLogo from "../../assets/signIn.svg";
import SignInFormData from "../../components/signin/SignInFormData";
import RegistrationForm from "../../components/signin/registrationForm/registrationForm";

function SignIn(): ReactNode {
  return (
    <div className={classes.contentContainer}>
      <div className={classes.signInImg}>
        <img src={SignInLogo} />
      </div>
      <div className={classes.rightBlock}>
        <SignInForm />
        <RegistrationForm />
      </div>
    </div>
  );
}

export default SignIn;
