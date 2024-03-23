import { useState } from "react";
import InputForm from "../../general/inputform/inputform";
import classes from "./resetPassword.module.css";
import Button from "../../general/button/button";
import axios from "axios";
import Config from "../../../config/config";
import ResetPasswordImg from "../../../assets/resetPassword.svg";
import { Link, Navigate, useNavigate, useSearchParams } from "react-router-dom";

type ResetPasswordFormData = {
  email: string;
  newPassword: string;
  resetCode: string;
};

function ResetPasswordComponent() {
  const navigate = useNavigate();
  const [searchParams, _] = useSearchParams();
  const [password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");
  const [error, setError] = useState<string | undefined>();
  const email = searchParams.get("email");
  const code = searchParams.get("code");

  if (email == null || code == null)
    return <Navigate to="/forgotPassword"></Navigate>;

  const handleSubmit = () => {
    if (password != repeatPassword) {
      setError("Пароли не совпадают.");
      return;
    }
    const formData: ResetPasswordFormData = {
      email: email,
      newPassword: password,
      resetCode: code,
    };

    console.log(formData);
    setError(undefined);
    axios
      .post(Config.UsersBackend + "resetPassword", formData)
      .then((_) => {
        navigate("/login");
      })
      .catch((error) => {
        console.log(error);
        if (
          error.response &&
          error.response.status == 400 &&
          error.response.data.errors
        ) {
          if (error.response.data.errors.InvalidToken) {
            setError(
              "Срок действия ссылки истек. Попробуйте запросить смену пароля еще раз."
            );
          } else if (error.response.data.errors.PasswordRequiresDigit) {
            setError("Пароль должен содержать хотя бы одну цифру.");
          } else if (
            error.response.data.errors.PasswordRequiresNonAlphanumeric
          ) {
            setError(
              "Пароль должен содержать хотя бы один специальный символ."
            );
          } else if (error.response.data.errors.PasswordRequiresUpper) {
            setError("Пароль должен содержать хотя бы одну заглавную букву.");
          } else if (error.response.data.errors.PasswordRequiresLower) {
            setError("Пароль должен содержать хотя бы одну прописную букву.");
          } else if (error.response.data.errors.PasswordTooShort) {
            setError("Пароль слишком короткий.");
          } else {
            setError("Пароль слишком простой.");
          }
        } else setError("Произошла ошибка.");
      });
  };

  return (
    <div className={classes.container}>
      <img className={classes.image} src={ResetPasswordImg} />
      <div className={classes.title}>Смените пароль от вашего аккаунта</div>
      <form
        className={classes.form}
        action="post"
        onSubmit={(e: React.FormEvent<HTMLFormElement>) => {
          e.preventDefault();
          handleSubmit();
        }}
      >
        <InputForm
          placeholder="Пароль"
          type="password"
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setPassword(e.target.value)
          }
        />
        <InputForm
          placeholder="Повторите пароль"
          type="password"
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setRepeatPassword(e.target.value)
          }
        />
        <Button type="submit">сменить пароль</Button>
      </form>
      {error != undefined ? <p className={classes.error}>{error}</p> : <></>}
      <Link to="/forgotPassword" className={classes.link}>
        Запросить сброс пароля
      </Link>
    </div>
  );
}

export default ResetPasswordComponent;
