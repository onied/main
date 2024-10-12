import classes from "./cardTwoFactor.module.css";
import Card from "../../../general/card/card";
import TwoFactorImg from "../../../../assets/twoFactor.svg";
import Button from "../../../general/button/button";
import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
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
      api
        .post("login", {
          email: email,
          password: password,
          twoFactorCode: digits,
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
        .catch((reason) => {
          const message =
            reason.response == null || reason.response.status >= 500
              ? "Ошибка сервера"
              : reason.response.data.detail == "LockedOut"
                ? "Слишком много неверных попыток входа, подождите несколько минут и попробуйте еще раз."
                : "Неверные данные для входа";

          navigator("/login", {
            state: {
              errorMessage: message,
            },
          });
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
