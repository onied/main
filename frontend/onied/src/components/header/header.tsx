import { Link, useNavigate } from "react-router-dom";
import classes from "./header.module.css";
import Logo from "../../assets/logo.svg";
import Avatar from "react-avatar";
import { useProfile } from "../../hooks/profile/useProfile";
import { getProfileName } from "../../hooks/profile/profile";
import NotificationContainer from "../notifications/notificationContainer";
import LoupeIcon from "../../assets/loupe.svg";
import { useState } from "react";

function Header() {
  const [profile, _] = useProfile();
  const [searchQuery, setSearchQuery] = useState("");
  const navigate = useNavigate();

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
          <Link to="/teaching" className={classes.link}>
            Преподавание
          </Link>
        </div>
      </div>
      <div className={classes.rightWrapper}>
        <div className={classes.searchWrapper}>
          <img className={classes.loupeIcon} src={LoupeIcon} />
          <input
            className={classes.search}
            type="text"
            placeholder={"Поиск..."}
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Enter" && searchQuery != "") {
                navigate(`/catalog?q=${searchQuery}`);
              }
            }}
          ></input>
        </div>
        {profile == null ? (
          <Link to="/login" className={classes.profileContainer}>
            Войти
          </Link>
        ) : (
          <>
            <NotificationContainer />
            <Link to="/profile" className={classes.profileContainer}>
              <p className={classes.profileName}>{getProfileName(profile)}</p>
              <Avatar
                name={getProfileName(profile)}
                size="50"
                className={classes.profileAvatar}
                src={profile.avatar ? profile.avatar : undefined}
              ></Avatar>
            </Link>
          </>
        )}
      </div>
    </header>
  );
}

export default Header;
