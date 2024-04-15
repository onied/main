import { Profile, getProfileName } from "../../../hooks/profile/profile";
import { CertificateCourse } from "../orderCertificate/orderCertificate";
import classes from "./printCertificate.module.css";

function PrintCertificate({
  course,
  profile,
}: {
  course: CertificateCourse;
  profile: Profile;
}) {
  return (
    <div className={classes.container}>
      <div className={classes.center}>
        <div className={classes.title}>Сертификат</div>
        <div className={classes.regular}>подтверждает, что</div>
        <div className={classes.user}>{getProfileName(profile)}</div>
        <div className={classes.regular}>прошел курс</div>
        <div className={classes.course}>“{course.title}”</div>
      </div>
      <div className={classes.corner}>
        <div className={classes.regular}>Автор курса:</div>
        <div className={classes.author}>
          {course.author.firstName + " " + course.author.lastName}
        </div>
      </div>
    </div>
  );
}

export default PrintCertificate;
