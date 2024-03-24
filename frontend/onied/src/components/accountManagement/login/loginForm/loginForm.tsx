import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";

import Button from "../../../general/button/button";
import InputForm from "../../../general/inputform/inputform";

import classes from "./loginForm.module.css";
import api from "../../../../config/axios";
import VkButton from "../../../general/vkbutton/vkbutton";
import LoginService from "../../../../services/loginService";

type LoginFormData = {
  email: string;
  password: string;
};

function LoginForm() {
  const navigator = useNavigate();
  const location = useLocation();

  const [errorMessage, setErrorMessage] = useState<string>();
  const [email, setEmail] = useState<string>();
  const [password, setPassword] = useState<string>();

  useEffect(() => {
    if (location.state != null) {
      setErrorMessage(location.state.errorMessage);
    }
  }, []);

  const handleSubmit = () => {
    const formData: LoginFormData = {
      email: email!,
      password: password!,
    };

    console.log(formData);

    api
      .get("manage/2fa/info", {
        params: { email: email },
      })
      .then((response) => {
        if (response.data.isTwoFactorEnabled == true) {
          navigator("/login/2fa", { state: { ...formData } });
        }

        return api.post("login", formData);
      })
      .then((response) => {
        console.log(response);
        LoginService.storeTokens(
          response.data.accessToken,
          response.data.expiresIn,
          response.data.refreshToken
        );
        navigator("/");
      })
      .catch((_) => {
        setErrorMessage("Неверные имя пользователя или пароль");
      });
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
        {errorMessage && (
          <span className={classes.errorMessage}>{errorMessage}</span>
        )}
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

      <VkButton></VkButton>

      <div className={classes.forgotPassword}>
        <Link to="/forgotPassword">Забыли пароль?</Link>
      </div>
    </div>
  );
}

export default LoginForm;
