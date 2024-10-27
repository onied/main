import classes from "./operatorsHeader.module.css"

import Logo from "@onied/assets/logo.svg"
import { OperatorProfile } from "@onied/types/chat"
import OperatorChatApi from "@onied/api/operatorChat"

import { useEffect, useState } from "react"
import { Link } from "react-router-dom"

export default function OperatorsHeader() {
    const [profile, setProfile] = useState<OperatorProfile | null>()

    const opApi = new OperatorChatApi();
    useEffect(() => {
        // opApi
        //     .GetOperatorProfile()
        //     .then(mbProfile => setProfile(mbProfile))
        setProfile({
            Number: 14
        })
    }, []);

    return (
        <header className={classes.header}>
            <div className={classes.logoContainer}>
                <Link to="/" className={classes.logoImage}><img src={Logo}></img></Link>
                <Link to="/operators"><h1 className={classes.logoTitle}>OniEd Operators</h1></Link>
            </div>
            <div className={classes.rightWrapper}>{
                profile == null
                    ? <Link to="/login" className={classes.profileContainer}>
                        Войти
                    </Link>
                    : <div className={classes.profileContainer}>
                        <p className={classes.profileName}>ОПЕРАТОР #{profile.Number}</p>
                    </div>
            }</div>
        </header>
    );
}
