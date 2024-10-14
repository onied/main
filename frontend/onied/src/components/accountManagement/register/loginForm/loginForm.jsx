import { useNavigate } from "react-router-dom";
import classes from "./loginForm.module.css";
import Button from "../../../general/button/button.tsx";

function LoginForm() {
  const navigator = useNavigate();

  return (
    <div className={classes.login}>
      <div style={{ whiteSpace: "nowrap", fontSize: "12pt" }}>
        Есть аккаунт?
      </div>
      <Button style={{ width: "100%" }} onClick={() => navigator("/login")}>
        войти
      </Button>
    </div>
  );
}

export default LoginForm;
