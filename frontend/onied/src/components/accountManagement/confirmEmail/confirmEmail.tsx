import classes from "./confirmEmail.module.css"
import ConfirmEmailImg from "../../../assets/confirmEmail.svg";
import SixDigitsInput from "../sixDigitsInput/sixDigitsInput";
import Button from "../../general/button/button";
import Card from "../../general/card/card";
import {useEffect, useState} from "react";
import {Navigate, useNavigate, useSearchParams} from "react-router-dom";
import api from "../../../config/axios";
import config from "../../../config/config";
import QRCode from "react-qr-code";

function ConfirmEmailComponent() {

  const navigator = useNavigate();
  const [searchParams, _] = useSearchParams();

  const [digits, setDigits] = useState("");
  const [sharedKey, setSharedKey] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [uriForQr, setUriForQr] = useState("");

  const [showKeyText, setShowKeyText] = useState(true);
  const showKey = () => {
    setShowKeyText(!showKeyText);
  };



  useEffect(() => {
    const userId = searchParams.get("userId");
    const code = searchParams.get("code");
    const accessToken = localStorage.getItem('access_token')

    if (userId == null || code == null || !accessToken)
      navigator("/login");

    api.defaults.headers.common['Authorization'] = 'Bearer ' + accessToken;
    api
      .get("confirmEmail", {
        params: {userId: userId, code: code},
      })
      .then((response) => {
        console.log(response);
      })
      .catch()


    api
      .post("manage/2fa", {})
      .then((response) => {
        setSharedKey(response.data.sharedKey);
        setUriForQr(`otpauth://totp/Onied:${123}?secret=${response.data.sharedKey}&issuer=Onied&digits=6`)
      })
      .catch()
  }, [])

  const sendCode = () => {
    setErrorMessage("")

    if (digits !== "") {
      api
        .post("manage/2fa", {
          enable: true,
          twoFactorCode: digits,
        })
        .then((response) => {
          console.log(response)
          navigator("/")
        })
        .catch((reason) => {
          const message =
            reason.response == null || reason.response.status >= 500
            ? "Ошибка сервера" : "Вы ввели неверный код";
          setErrorMessage(message)
          console.log(reason)
        });
    } else {
      setErrorMessage("Поле не заполнено");
    }
  };

  return (
      <>
        <div>
          <Card className={classes.cardConfirmEmail}>
            <div className={classes.confirmEmailImg}>
              <img src={ConfirmEmailImg} />
            </div>
            <div className={classes.textInfo}>
              <div className={classes.title}>
                Почта была успешно подтверждена
              </div>
              <div className={classes.pleaseEnter}>
                Onied использует двухфакторную аутентификацию по умолчанию. <br/>
                отсканируйте QR-код в приложении аутентификации.
              </div>
              <div className={classes.qrCode}>
                <QRCode value={uriForQr}></QRCode>
              </div>
              <div className={classes.pleaseEnter}>
                или введите ключ вручную: <br />
                <button className={classes.showKey} onClick={showKey}>
                  {showKeyText ? 'показать ключ' : sharedKey}
                </button>
              </div>
              <div className={classes.pleaseEnter}>
                Для проверки, введите код из приложения:
              </div>
              <SixDigitsInput onChange={setDigits}/>
              {errorMessage && (
                <span className={classes.errorMessage}>{errorMessage}</span>
              )}
              <Button style={{margin: "auto", width: "60%"}} onClick={sendCode}>
                подтвердить
              </Button>
            </div>
          </Card>
        </div>
      </>
  )
}

export default ConfirmEmailComponent;
