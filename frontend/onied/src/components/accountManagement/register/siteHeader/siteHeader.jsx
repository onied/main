import VkButton from "../../../general/vkbutton/vkbutton";
import classes from "./siteHeader.module.css";

function SiteHeader() {
  return (
    <div>
      <div className={classes.siteHeader}>
        <div className={classes.title}>OniEd</div>
        <div className={classes.textInfo}>
          Зарегистрируйтесь и получите доступ <br />к множеству онлайн курсов
        </div>
        <VkButton></VkButton>
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
