import { useState } from "react";

import Button from "../../general/button/button";
import InputForm from "../../general/inputform/inputform";

import classes from "./signInForm.module.css";
import VkLogo from "../../../assets/vk.svg";

type SignInFormData = {
  email: string;
  password: string;
};

function SignInForm({ onFormSubmit }) {
  const [email, setEmail] = useState<string>();
  const [password, setPassword] = useState<string>();

  const handleSubmit = () => {
    const formData: SignInFormData = {
      email: email!,
      password: password!,
    };

    onFormSubmit(formData);
  };

  return (
    <>
      <div className={classes.title}>OniEd</div>

      <form
        className={classes.form}
        action="post"
        onSubmit={(e: React.FormEvent<HTMLFormElement>) => {
          e.preventDefault();
          handleSubmit();
        }}
      >
        <InputForm
          placeholder="Эл. адрес"
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setEmail(e.target.value)
          }
        />
        <InputForm
          placeholder="Пароль"
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setPassword(e.target.value)
          }
        />
        <Button style={{ width: "100%" }}>войти</Button>
      </form>

      <div className={classes.or}>
        <div className={classes.line}></div>
        <div style={{ color: "#494949" }}>или</div>
        <div className={classes.line}></div>
      </div>

      <div className={classes.authVK}>
        <img src={VkLogo} />
        <div>Войти через VK</div>
      </div>
    </>
  );
}

export default SignInForm;
