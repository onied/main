import { Link } from "react-router-dom";
import classes from "./header.module.css";
import Logo from "../../assets/logo.svg";
import Avatar from "react-avatar";
import { useState } from "react";

function Header() {
  const [profileInfo, setProfileInfo] = useState({
    name: "Иван Иванов",
  });
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
        <Link to="/profile" className={classes.profileContainer}>
          <p className={classes.profileName}>{profileInfo.name}</p>
          <Avatar
            name={profileInfo.name}
            size="50"
            className={classes.profileAvatar}
          ></Avatar>
        </Link>
      </div>
    </header>
  );
}

export default Header;
