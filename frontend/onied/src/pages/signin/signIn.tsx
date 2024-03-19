import { ReactNode, useEffect, useState } from "react";
import { redirect, useParams } from "react-router-dom";

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
  const [errorMessage, setErrorMessage] = useState<string>();

  const trySignIn = (formData: SignInDto) => {
    console.log(formData);

    axios
      .post(Config.UsersBackend + "sign_in", formData)
      .then((response) => {
        console.log(response.data);

        // savin' data

        redirect("/");
      })
      .catch((error) => {
        console.log(error);
        setErrorMessage("Неверные данные для входа");
      });
  };

  return (
    <div className={classes.contentContainer}>
      <div className={classes.signInImg}>
        <img src={SignInLogo} />
      </div>
      <div className={classes.rightBlock}>
        {errorMessage === undefined ? null : <div>{errorMessage}</div>}
        <SignInForm onFormSubmit={trySignIn} />
      </div>
    </div>
  );
}

export default SignIn;
