import classes from "./cardTwoFactor.module.css";
import Card from "../../../general/card/card.jsx";
import TwoFactorImg from "../../../../assets/twoFactor.svg";
import SingleDigitInput from "../singleDigitInput/singleDigitInput.jsx";
import Button from "../../../general/button/button.jsx";
import {useRef, useState} from "react";
import axios from "axios";
import config from "../../../../config/config.js";
import {handleErrors} from "../../register/registrationForm/validation.js";
import {useNavigate} from "react-router-dom";


function CardTwoFactor() {
  const navigator = useNavigate();
  const inputsRefs = useRef([]);

  const [digits, setDigits] = useState(['', '', '', '', '', '']);
  const [errorMessage, setErrorMessage] = useState('');

  const handleChange = (index, event) => {
    const value = event.target.value
    if (value === '') {
      handleBackspace(index);
    } else if (/^\d$/.test(value)) {
      handleInput(index, value);
    }
  }

  const handleInput = (index, event) => {
    const value = event.value
    setDigits(prevDigits => {
      const newDigits = [...prevDigits];
      newDigits[index] = value;
      return newDigits;
    });
    const nextIndex = index + 1;
    if (nextIndex < inputsRefs.current.length) {
      inputsRefs.current[nextIndex].focus();
    }
  };

  const handleBackspace = (index) => {
    setDigits(prevDigits => {
      const newDigits = [...prevDigits];
      newDigits[index] = '';
      return newDigits;
    });
    const prevIndex = index - 1;
    if (prevIndex >= 0) {
      inputsRefs.current[prevIndex].focus();
    }
  };

  const sendCode = () => {
    if (digits.every(digit => digit !== '')) {
      axios
        .post(config.Users + "manage/2fa", {
          enable: true,
          twoFactorCode: digits.join(''),
          resetSharedKey: false,
          resetRecoveryCodes: false,
          forgetMachine: false
        })
        .then((response) => {
          console.log(response);
          navigator("/");
        })
        .catch((error) => {
          console.log(error);
          setErrorMessage("Неверный код")
        });
    } else {
      setErrorMessage("Неверный код")
    }
  }

  return(
    <>
      <Card className={classes.cardTwoFactor}>
        <div className={classes.twoFactorImg} >
          <img src={TwoFactorImg}/>
        </div>
        <div className={classes.textInfo}>
          <div className={classes.title}>
            Аутентифицируйте свою учетную <br/>запись
          </div>
          <div className={classes.pleaseEnterCode}>
            Введите код, сгенерированный вашим приложением <br/>для 2-факторной аутентификации
          </div>
          <div className={classes.inputDigits}>
            {[...Array(6).keys()].map((index) => (
              <SingleDigitInput
                key={index}
                id={`input-${index}`}
                innerRef={(input) => (inputsRefs.current[index] = input)}
                onChange={(event) => handleChange(index, event)}
                className={classes.singleDigitInput}
                value={digits[index]}
              />
            ))}
          </div>
          {errorMessage && (
            <span className={classes.errorMessage}>{errorMessage}</span>)}
          <Button style={{margin: "auto", width:"60%"}} onClick={sendCode}>
            подтвердить
          </Button>
        </div>
      </Card>
    </>
  )
}

export default CardTwoFactor;
