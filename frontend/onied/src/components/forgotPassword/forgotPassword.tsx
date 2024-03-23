import { useState } from "react";
import InputForm from "../general/inputform/inputform";
import classes from "./forgotPassword.module.css";
import Button from "../general/button/button";
import axios from "axios";
import Config from "../../config/config";
import ForgotPasswordImg from "../../assets/forgotPassword.svg";

type ForgotPasswordFormData = {
  email: string;
};

function ForgotPasswordComponent() {
  const [email, setEmail] = useState("");
  const [sent, setSent] = useState<Boolean>();
  const [error, setError] = useState<string | undefined>();
  const [buttonDisabled, setButtonDisabled] = useState("");

  const handleSubmit = () => {
    const formData: ForgotPasswordFormData = {
      email: email!,
    };

    console.log(formData);
    setError(undefined);
    axios
      .post(Config.Users + "forgotPassword", {
        email: email,
      })
      .then((_) => {
        setSent(true);
        setButtonDisabled("t");
      })
      .catch((error) => {
        setError("Произошла ошибка.");
        console.log(error);
      });
  };

  return (
    <div className={classes.container}>
      <img className={classes.image} src={ForgotPasswordImg} />
      <div className={classes.title}>Забыли пароль?</div>
      <div className={classes.description}>
        Введите электронную почту. Ссылка для замены пароля будет отправлена на
        указанную электронную почту
      </div>

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
          type="email"
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setEmail(e.target.value)
          }
        />
        <Button type="submit" disabled={buttonDisabled}>
          отправить
        </Button>
      </form>
      {sent ? (
        <p className={classes.sent}>
          Письмо отправлено на электронный ящик пользователя с указанной почтой.
        </p>
      ) : (
        <></>
      )}
      {error != undefined ? <p className={classes.sent}>{error}</p> : <></>}
    </div>
  );
}

export default ForgotPasswordComponent;
