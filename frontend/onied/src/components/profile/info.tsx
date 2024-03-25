import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";

function ProfileInfo() {
  const profile = useProfile();
  if (profile == null) return <></>;
  return (
    <div className={classes.pageWrapper}>
      <h3 className={classes.pageTitle}>Редактирование профиля</h3>
      <form>
        <div className={classes.profileInfoGrid}></div>
      </form>
    </div>
  );
}

export default ProfileInfo;
