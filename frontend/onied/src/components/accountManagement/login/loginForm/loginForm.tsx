import { useEffect, useState } from "react";
import {
  Link,
  useLocation,
  useNavigate,
  useSearchParams,
} from "react-router-dom";

import Button from "../../../general/button/button";
import InputForm from "../../../general/inputform/inputform";

import classes from "./loginForm.module.css";
import api from "../../../../config/axios";
import VkButton from "../../../general/vkbutton/vkbutton";
import LoginService from "../../../../services/loginService";
import ProfileService from "../../../../services/profileService";
import { AxiosError } from "axios";

type LoginFormData = {
  email: string;
  password: string;
  twoFactorCode?: string | undefined;
};

function LoginForm() {
  const [searchParams, _] = useSearchParams();
  const navigator = useNavigate();
  const location = useLocation();

  const [errorMessage, setErrorMessage] = useState<string>();
  const [email, setEmail] = useState<string>(location.state?.email);
  const [password, setPassword] = useState<string>(location.state?.password);

  const redirect = location.state?.redirect ?? searchParams.get("redirect");

  useEffect(() => {
    if (location.state) {
      handleSubmit(location.state.email, location.state.password);
    }
  }, [location.state]);

  const handleSubmit = (email: string, password: string) => {
    const formData: LoginFormData = {
      email: email,
      password: password,
      twoFactorCode: location.state?.twoFactorCode ?? undefined,
    };
    api
      .post("login", formData)
      .then((response) => {
        LoginService.storeTokens(
          response.data.accessToken,
          response.data.expiresIn,
          response.data.refreshToken
        );
        ProfileService.fetchProfile();
        if (redirect) {
          const decoded = decodeURIComponent(redirect);
          if (decoded.startsWith("/")) {
            navigator(decoded);
            return;
          }
        }
        navigator("/");
      })
      .catch((error: AxiosError<any, any>) => {
        if (!error.response || error.response.status >= 500)
          setErrorMessage(
            "Что-то пошло не так. Проверьте соединение с интернетом или повторите попытку позже."
          );
        else {
          if (
            error.response.status === 401 &&
            error.response.data.detail === "RequiresTwoFactor"
          ) {
            navigator("/login/2fa", {
              state: { ...formData, redirect: redirect },
            });
          } else if (error.response.data.detail === "LockedOut") {
            setErrorMessage(
              "Слишком много неверных попыток входа, повторите попытку позже."
            );
          } else if (formData.twoFactorCode) {
            setErrorMessage("Неверные данные для входа");
          } else {
            setErrorMessage("Неверные имя пользователя или пароль");
          }
        }
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
          handleSubmit(email!, password!);
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
