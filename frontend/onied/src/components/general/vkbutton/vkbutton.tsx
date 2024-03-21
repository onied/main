import classes from "./vkbutton.module.css";
import VkLogo from "../../../assets/vk.svg";
import Config from "../../../config/config";

function VkButton() {
  return (
    <a
      className={classes.authVK}
      href={`https://oauth.vk.com/authorize?client_id=${Config.ClientId}&display=page&redirect_uri=${Config.RedirectUrl}`}
    >
      <img src={VkLogo} />
      <div>Войти через VK</div>
    </a>
  );
}

export default VkButton;
