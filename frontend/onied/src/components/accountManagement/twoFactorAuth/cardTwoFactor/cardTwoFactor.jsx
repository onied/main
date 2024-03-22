import classes from "./cardTwoFactor.module.css";
import Card from "../../../general/card/card.jsx";
import TwoFactorImg from "../../../../assets/twoFactor.svg";

function CardTwoFactor() {
  const styleCard = {
    display: "flex",
    flexDirection: "column"
  }

  return(
    <>
      <Card style={{...styleCard}}>
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
        </div>
      </Card>
    </>
  )
}

export default CardTwoFactor;
