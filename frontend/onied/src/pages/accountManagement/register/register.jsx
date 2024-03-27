import RegistrationForm from "../../../components/accountManagement/register/registrationForm/registrationForm.jsx";
import LoginForm from "../../../components/accountManagement/register/loginForm/loginForm.jsx";
import classes from "./register.module.css";
import SiteHeader from "../../../components/accountManagement/register/siteHeader/siteHeader.jsx";
import Card from "../../../components/general/card/card.jsx";

function Register() {
  return (
    <div className={classes.container}>
      <div>
        <Card>
          <SiteHeader />
          <RegistrationForm />
        </Card>
        <LoginForm />
      </div>
    </div>
  );
}

export default Register;
