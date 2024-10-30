import classes from "./operatorsHeader.module.css";

import Logo from "@onied/assets/logo.svg";
import OperatorChatApi from "@onied/api/operatorChat";

import { useEffect } from "react";
import { Link, useLocation } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "@onied/hooks";
import { ChatsStateActionTypes } from "@onied/redux/reducers/chatReducer";

export default function OperatorsHeader() {
  const chatsState = useAppSelector((state) => state.chats);
  const dispatch = useAppDispatch();
  const location = useLocation();

  const opApi = new OperatorChatApi();
  useEffect(() => {
    opApi.GetOperatorProfile().then((mbProfile) =>
      dispatch({
        type: ChatsStateActionTypes.FETCH_OPERATOR_PROFILE,
        payload: mbProfile,
      })
    );
  }, []);

  return (
    <header className={classes.header}>
      <div className={classes.logoContainer}>
        <Link to="/" className={classes.logoImage}>
          <img src={Logo}></img>
        </Link>
        <Link to="/operators">
          <h1 className={classes.logoTitle}>OniEd Operators</h1>
        </Link>
      </div>
      <div className={classes.rightWrapper}>
        {chatsState.operatorProfile ? (
          <div className={classes.profileContainer}>
            <p className={classes.profileName}>
              ОПЕРАТОР #{chatsState.operatorProfile.number}
            </p>
          </div>
        ) : (
          <Link
            to={`/login?redirect=${encodeURIComponent(location.pathname)}`}
            className={classes.profileContainer}
          >
            Войти
          </Link>
        )}
      </div>
    </header>
  );
}
