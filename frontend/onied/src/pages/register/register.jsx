import { Link } from "react-router-dom";
import Button from "../../components/general/button/button";
import InputForm from "../../components/general/inputform/inputform.jsx";
import Radio from "../../components/general/radio/radio";
import RegistrationForm from "../../components/register/registrationForm/registrationForm.jsx";
import LoginForm from "../../components/register/loginForm/loginForm.jsx";
import classes from "./register.module.css";
import SiteHeader from "../../components/register/siteHeader/siteHeader.jsx";

function Register() {
  return (
    <div className={classes.container}>
      <div>
        <div className={classes.cardForm}>
          <SiteHeader />
          <RegistrationForm />
        </div>
        <LoginForm />
      </div>
    </div>
  );
}

export default Register;
