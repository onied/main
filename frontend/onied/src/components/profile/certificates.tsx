import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";

function ProfileCertificates() {
  const profile = useProfile();
  if (profile == null) return <></>;
  return (
    <div className={classes.pageWrapper}>
      <h3 className={classes.pageTitle}>Доступные сертификаты</h3>
    </div>
  );
}

export default ProfileCertificates;
