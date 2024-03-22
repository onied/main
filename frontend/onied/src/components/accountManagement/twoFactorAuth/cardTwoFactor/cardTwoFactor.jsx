import classes from "./cardTwoFactor.module.css";
import Card from "../../../general/card/card.jsx";
import TwoFactorImg from "../../../../assets/twoFactor.svg";
import SingleDigitInput from "../singleDigitInput/singleDigitInput.jsx";
import Button from "../../../general/button/button.jsx";


function CardTwoFactor() {


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
                className={classes.singleDigitInput}
              />
            ))}
          </div>
          <Button style={{margin: "auto", width:"45%"}}>
            подтвердить
          </Button>
        </div>
      </Card>
    </>
  )
}

export default CardTwoFactor;
