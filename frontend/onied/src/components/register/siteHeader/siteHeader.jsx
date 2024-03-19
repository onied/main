import classes from "../siteHeader/siteHeader.module.css";
import VkLogo from "../../../assets/vk.svg";

function SiteHeader() {
  return (
    <div>
      <div className={classes.siteHeader}>
        <div className={classes.title}>OniEd</div>
        <div className={classes.textInfo}>
          Зарегистрируйтесь и получите доступ <br />к множеству онлайн курсов
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
    </div>
  );
}

export default SiteHeader;
