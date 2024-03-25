import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";

function ProfileCourses() {
  const profile = useProfile();
  if (profile == null) return <></>;
  return (
    <div className={classes.pageWrapper}>
      <h3 className={classes.pageTitle}>Доступные курсы</h3>
    </div>
  );
}

export default ProfileCourses;
