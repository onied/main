import InputForm from "../../general/inputform/inputform.jsx";
import Radio from "../../general/radio/radio.jsx";
import Button from "../../general/button/button";
import classes from "./registrationForm.module.css";
import axios from "axios";
import Config from "../../../config/config.js";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { handleErrors, validateInput } from "./validation.js";

function RegistrationForm() {
  const navigator = useNavigate();

  const [gender, setGender] = useState(0);
  const genders = {
    "Не указан": 0,
    Мужской: 1,
    Женский: 2,
  };

  const [input, setInput] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const [errorMessage, setErrorMessage] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const onInputChange = (e) => {
    const { name, value } = e.target;
    setInput((prev) => ({
      ...prev,
      [name]: value,
    }));
    setErrorMessage((prev) => ({ ...prev, [e.target.name]: "" }));
  };

  const handleToggle = (event) => {
    setGender(genders[event.target.name]);
  };

  useEffect(() => {
    const hasError = Object.keys(errorMessage).some((key) => errorMessage[key]);
    setIsFormValid(!hasError);
  }, [errorMessage]);

  const [isFormValid, setIsFormValid] = useState(false);

  const onFinish = (event) => {
    event.preventDefault();
    const form = event.target;
    const fields = form.elements;

    for (let i = 0; i < fields.length; i++) {
      validateInput(fields[i], input, setErrorMessage);
    }

    if (isFormValid) {
      let firstName = form.firstName.value;
      let lastName = form.lastName.value;
      let email = form.email.value;
      let password = form.password.value;

      axios
        .post(Config.UsersBackend + "register", {
          firstName,
          lastName,
          gender,
          email,
          password,
        })
        .then((response) => {
          console.log(response);
          navigator("/login");
        })
        .catch((error) => {
          console.log(error);
          handleErrors(error.response.data.errors, setErrorMessage);
        });
    }
  };

  return (
    <div>
      <form className={classes.form} onSubmit={onFinish}>
        <InputForm
          placeholder={"Имя"}
          name={"firstName"}
          onChange={onInputChange}
        ></InputForm>
        {errorMessage.firstName && (
          <span className={classes.errorMessage}>{errorMessage.firstName}</span>
        )}
        <InputForm
          placeholder={"Фамилия"}
          name={"lastName"}
          onChange={onInputChange}
        ></InputForm>
        {errorMessage.lastName && (
          <span className={classes.errorMessage}>{errorMessage.lastName}</span>
        )}
        <div className={classes.genders}>
          <div className={classes.blockGender}>
            <label className={classes.labelGenders}>Пол</label>
          </div>
          <div className={classes.variants}>
            {Object.keys(genders).map((key) => (
              <div className={classes.gender}>
                <label className={classes.labelGenders}>{key}</label>
                <Radio
                  id={genders[key]}
                  name={key}
                  onChange={handleToggle}
                  checked={gender == genders[key] ? "t" : ""}
                ></Radio>
              </div>
            ))}
          </div>
        </div>
        <InputForm
          placeholder={"Эл. адрес"}
          name={"email"}
          onChange={onInputChange}
        ></InputForm>
        {errorMessage.email && (
          <span className={classes.errorMessage}>{errorMessage.email}</span>
        )}
        <InputForm
          placeholder={"Пароль"}
          name={"password"}
          type="password"
          onChange={onInputChange}
        ></InputForm>
        {errorMessage.password && (
          <span className={classes.errorMessage}>{errorMessage.password}</span>
        )}
        <InputForm
          placeholder={"Повторите пароль"}
          name={"confirmPassword"}
          type="password"
          onChange={onInputChange}
        ></InputForm>
        {errorMessage.confirmPassword && (
          <span className={classes.errorMessage}>
            {errorMessage.confirmPassword}
          </span>
        )}
        <Button style={{ margin: "auto", width: "60%" }}>далее</Button>
      </form>
    </div>
  );
}

export default RegistrationForm;
