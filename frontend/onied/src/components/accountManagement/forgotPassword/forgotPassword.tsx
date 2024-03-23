import { useState } from "react";
import InputForm from "../../general/inputform/inputform";
import classes from "./forgotPassword.module.css";
import Button from "../../general/button/button";
import axios from "axios";
import Config from "../../../config/config";
import ForgotPasswordImg from "../../../assets/forgotPassword.svg";

type ForgotPasswordFormData = {
  email: string;
};

function ForgotPasswordComponent() {
  const [email, setEmail] = useState("");
  const [sent, setSent] = useState<Boolean>();
  const [error, setError] = useState<string | undefined>();
  const [buttonDisabled, setButtonDisabled] = useState("");

  const handleSubmit = () => {
    if (
      !/(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])/.test(
        email
      )
    ) {
      setError("Вы ввели неправильный электронный адрес");
      return;
    }
    const formData: ForgotPasswordFormData = {
      email: email!,
    };

    console.log(formData);
    setError(undefined);
    axios
      .post(Config.UsersBackend + "forgotPassword", formData)
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
          required="t"
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
      {error != undefined ? <p className={classes.error}>{error}</p> : <></>}
    </div>
  );
}

export default ForgotPasswordComponent;
