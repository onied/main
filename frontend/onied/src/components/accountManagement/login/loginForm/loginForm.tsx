import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";

import Button from "../../../general/button/button";
import InputForm from "../../../general/inputform/inputform";

import classes from "./loginForm.module.css";
import VkLogo from "../../../../assets/vk.svg";
import axios from "axios";

type LoginFormData = {
  email: string;
  password: string;
};

function LoginForm() {
  const navigator = useNavigate();

  const [email, setEmail] = useState<string>();
  const [password, setPassword] = useState<string>();

  const handleSubmit = () => {
    const formData: LoginFormData = {
      email: email!,
      password: password!,
    };

    console.log(formData);

    axios
      .get("/manage/2fa/info", { params: { email: email } })
      .then((response) => {
        if (response.data.isTwoFactorEnabled == true) {
          navigator("/login/2fa", { state: { ...formData } });
        }

        return axios.post("/login", formData);
      })
      .then((response) => {
        navigator("/");
      })
      .catch();
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
