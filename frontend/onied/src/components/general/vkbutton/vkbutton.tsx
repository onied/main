import classes from "./vkbutton.module.css";
import VkLogo from "../../../assets/vk.svg";
import Config from "../../../config/config";

function VkButton() {
  const url = new URL(Config.VkAuthorizationUrl);
  url.searchParams.append("client_id", Config.ClientId);
  url.searchParams.append("redirect_uri", Config.RedirectUrl);
  url.searchParams.append("display", "page");
  url.searchParams.append("scope", "email");
  return (
    <a className={classes.authVK} href={url.toString()}>
      <img src={VkLogo} />
      <div>Войти через VK</div>
    </a>
  );
}

export default VkButton;
