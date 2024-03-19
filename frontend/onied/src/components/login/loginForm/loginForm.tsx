import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";

import axios, { AxiosError } from "axios";

import Button from "../../general/button/button";
import InputForm from "../../general/inputform/inputform";

import classes from "./loginForm.module.css";
import VkLogo from "../../../assets/vk.svg";
import Config from "../../../config/config";

type LoginFormData = {
  email: string;
  password: string;
};

function LoginForm() {
  const navigator = useNavigate();

  const [email, setEmail] = useState<string>();
  const [password, setPassword] = useState<string>();

  // const [errorMessage, setErrorMessage] = useState<string>();

  const handleSubmit = () => {
    const formData: LoginFormData = {
      email: email!,
      password: password!,
    };

    console.log(formData);
    navigator("/login/2fa", { state: { ...formData } });
    // axios
    //   .post(Config.Users + "login", formData)
    //   .then((response) => {
    //     console.log(response.data);

    //     // maybe get some check from backend?
    //   })
    //   .catch((error: AxiosError) => {
    //     console.log(error);

    //     let message = "Неизвестная ошибка";

    //     if (error.response == null) {
    //       setErrorMessage("Нет ответа от сервера");
    //       return;
    //     }

    //     const statusCode = error.response!.status;
    //     if (statusCode === 401) {
    //       message = "Неверные данные для входа";
    //     } else if (statusCode >= 500) {
    //       message = "Произошла ошибка на сервере";
    //     }
    //     setErrorMessage(message);
    //   });
  };

  return (
    <div className={classes.container}>
      <div className={classes.title}>OniEd</div>

      <form
        className={classes.form}
        action="post"
        onSubmit={(e: React.FormEvent<HTMLFormElement>) => {
          e.preventDefault();
          handleSubmit();
        }}
      >
        {/* {errorMessage === undefined ? null : (
          <span className={classes.errorMessage}>{errorMessage}</span>
        )} */}
        <InputForm
          placeholder="Эл. адрес"
          type="email"
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setEmail(e.target.value)
          }
        />
        <InputForm
          placeholder="Пароль"
          type="password"
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

      <div className={classes.forgotPassword}>
        <Link to="/forgotPassword">Забыли пароль?</Link>
      </div>
    </div>
  );
}

export default LoginForm;
