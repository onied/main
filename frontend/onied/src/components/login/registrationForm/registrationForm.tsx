import { useNavigate } from "react-router-dom";
import Button from "../../general/button/button";
import classes from "./registrationForm.module.css";

function RegistrationForm() {
  const navigator = useNavigate();

  return (
    <div className={classes.container}>
      <div style={{ whiteSpace: "nowrap", fontSize: "12pt" }}>
        Нет аккаунта?
      </div>
      <Button style={{ width: "100%" }} onClick={() => navigator("/register")}>
        зарегистрироваться
      </Button>
    </div>
  );
}

export default RegistrationForm;
