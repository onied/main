import classes from "./twoFactor.module.css";
import CardTwoFactor from "../../../components/accountManagement/twoFactorAuth/cardTwoFactor/cardTwoFactor.jsx";
import { useLocation, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
function TwoFactor() {
  const navigator = useNavigate();
  const location = useLocation();

  const [email, setEmail] = useState();
  const [password, setPassword] = useState();

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

  return (
    <>
      <div className={classes.container}>
        <div>
          <CardTwoFactor email={email} password={password} />
        </div>
      </div>
    </>
  );
}

export default TwoFactor;
