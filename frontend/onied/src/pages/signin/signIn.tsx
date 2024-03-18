import { ReactNode, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import BeatLoader from "react-spinners/BeatLoader";

import axios from "axios";

import Config from "../../config/config";
import classes from "./signIn.module.css";
import InputForm from "../../components/general/inputform/inputform";
import Button from "../../components/general/button/button";
import SignInForm from "../../components/signin/signInForm/signInForm";

type SignInDto = {
  email: string;
  password: string;
};

function SignIn(): ReactNode {
  return (
    <>
      <SignInForm
        onFormSubmit={(formData: SignInDto) => {
          console.log(formData);
        }}
      />
    </>
  );
}

export default SignIn;
