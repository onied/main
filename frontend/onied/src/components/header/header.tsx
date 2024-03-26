import { Link } from "react-router-dom";
import classes from "./header.module.css";
import Logo from "../../assets/logo.svg";
import Avatar from "react-avatar";
import { useProfile } from "../../hooks/profile/useProfile";
import { getProfileName } from "../../hooks/profile/profile";

function Header() {
  const [profile, _] = useProfile();
  return (
    <header className={classes.header}>
      <div className={classes.leftWrapper}>
        <Link to="/" className={classes.logoContainer}>
          <div className={classes.logoImage}>
            <img src={Logo}></img>
          </div>
          <h1 className={classes.logoTitle}>OniEd</h1>
        </Link>
        <div className={classes.links}>
          <Link to="/catalog" className={classes.link}>
            Каталог
          </Link>
        </div>
      </div>
      <div className={classes.rightWrapper}>
        {profile == null ? (
          <Link to="/login" className={classes.profileContainer}>
            Войти
          </Link>
        ) : (
          <Link to="/profile" className={classes.profileContainer}>
            <p className={classes.profileName}>{getProfileName(profile)}</p>
            <Avatar
              name={getProfileName(profile)}
              size="50"
              className={classes.profileAvatar}
              src={profile.avatarHref ? profile.avatarHref : undefined}
            ></Avatar>
          </Link>
        )}
      </div>
    </header>
  );
}

export default Header;
