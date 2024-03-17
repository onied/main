import InputForm from "../../general/inputform/inputform.jsx";
import Radio from "../../general/radio/radio.jsx";
import Button from "../../general/button/button";
import classes from "./registrationForm.module.css";
import VkLogo from "../../../assets/vk.svg";

function RegistrationForm() {
  return (
    <div className={classes.container}>
      <div className={classes.card}>
        <div className={classes.cardForm}>
          <div className={classes.main}>
            <div className={classes.title}>OniEd</div>
            <div className={classes.textInfo}>
              Зарегистрируйтесь и получите доступ <br />к множеству онлайн
              курсов
            </div>
            <div className={classes.authVK}>
              <img src={VkLogo} />
              <div>Войти через VK</div>
            </div>
          </div>
          <div className={classes.or}>
            <div className={classes.line}></div>
            <div style={{ color: "#494949" }}>или</div>
            <div className={classes.line}></div>
          </div>
          <form className={classes.form}>
            <InputForm placeholder={"Имя"}></InputForm>
            <InputForm placeholder={"Фамилия"}></InputForm>
            <div className={classes.genders}>
              <div className={classes.blockGender}>
                <label className={classes.labelGenders}>Пол</label>
              </div>
              <div className={classes.variants}>
                <div className={classes.gender}>
                  <label className={classes.labelGenders}>Не указан</label>
                  <Radio></Radio>
                </div>
                <div className={classes.gender}>
                  <label className={classes.labelGenders}>Мужской</label>
                  <Radio></Radio>
                </div>
                <div className={classes.gender}>
                  <label className={classes.labelGenders}>Женский</label>
                  <Radio></Radio>
                </div>
              </div>
            </div>
            <InputForm placeholder={"Эл. адрес"}></InputForm>
            <InputForm placeholder={"Пароль"}></InputForm>
            <InputForm placeholder={"Повторите пароль"}></InputForm>
            <Button style={{ margin: "auto", width: "60%" }}>далее</Button>
          </form>
        </div>
        <div className={classes.login}>
          <div style={{ whiteSpace: "nowrap", fontSize: "12pt" }}>
            Есть аккаунт?
          </div>
          <Button style={{ width: "100%" }}>войти</Button>
        </div>
      </div>
    </div>
  );
}

export default RegistrationForm;
