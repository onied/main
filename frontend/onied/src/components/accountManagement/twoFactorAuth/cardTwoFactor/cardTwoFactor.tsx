import classes from "./cardTwoFactor.module.css";
import Card from "../../../general/card/card";
import TwoFactorImg from "../../../../assets/twoFactor.svg";
import Button from "../../../general/button/button";
import { useState, useEffect } from "react";
import { useNavigate, useLocation, redirect } from "react-router-dom";
import SixDigitsInput from "../../sixDigitsInput/sixDigitsInput";
import api from "../../../../config/axios";
import LoginService from "../../../../services/loginService";

function CardTwoFactor() {
  const navigator = useNavigate();
  const location = useLocation();

  const [digits, setDigits] = useState("");
  const [email, setEmail] = useState();
  const [password, setPassword] = useState();
  const [errorMessage, setErrorMessage] = useState("");

  useEffect(() => {
    if (
      location.state == null ||
      location.state.email == null ||
      location.state.password == null
    ) {
      navigator("/login");
    } else {
      setEmail(location.state.email);
      setPassword(location.state.password);
    }
  }, []);

  const sendCode = () => {
    if (digits !== "") {
      navigator("/login", {
        state: {
          email: email,
          password: password,
          twoFactorCode: digits,
          redirect: location.state.redirect,
        },
      });
    } else {
      setErrorMessage("Поле не заполнено");
    }
  };

  return (
    <>
      <Card className={classes.cardTwoFactor}>
        <div className={classes.twoFactorImg}>
          <img src={TwoFactorImg} />
        </div>
        <div className={classes.textInfo}>
          <div className={classes.title}>
            Аутентифицируйте свою учетную <br />
            запись
          </div>
          <div className={classes.pleaseEnterCode}>
            Введите код, сгенерированный вашим приложением <br />
            для 2-факторной аутентификации
          </div>
          <SixDigitsInput onChange={setDigits} />
          {errorMessage && (
            <span className={classes.errorMessage}>{errorMessage}</span>
          )}
          <Button style={{ margin: "auto", width: "60%" }} onClick={sendCode}>
            подтвердить
          </Button>
        </div>
      </Card>
    </>
  );
}

export default CardTwoFactor;
