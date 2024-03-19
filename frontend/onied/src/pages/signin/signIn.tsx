import { ReactNode, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import axios from "axios";

import Config from "../../config/config";
import classes from "./signIn.module.css";
import SignInForm from "../../components/signin/signInForm/signInForm";
import SignInLogo from "../../assets/signIn.svg";

type SignInDto = {
  email: string;
  password: string;
};

function SignIn(): ReactNode {
  return (
    <div className={classes.contentContainer}>
      <div className={classes.signInImg}>
        <img src={SignInLogo} />
      </div>
      <div className={classes.rightBlock}>
        <SignInForm
          onFormSubmit={(formData: SignInDto) => {
            console.log(formData);
          }}
        />
      </div>
    </div>
  );
}

export default SignIn;
