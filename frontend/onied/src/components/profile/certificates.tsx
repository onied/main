import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import CertificateContainer, { Certificate } from "./certificate/certificate";
import { useState } from "react";

function ProfileCertificates() {
  const profile = useProfile();
  const [certificateList, setCertificateList] = useState<
    Array<Certificate> | undefined
  >();
  if (profile == null) return <></>;
  return (
    <div className={classes.pageWrapper}>
      <h3 className={classes.pageTitle}>Доступные сертификаты</h3>
      {certificateList ? (
        certificateList.map((cert, index) => (
          <CertificateContainer
            certificate={cert}
            key={index}
          ></CertificateContainer>
        ))
      ) : (
        <p className={classes.noCourses}>Нет доступных сертификатов</p>
      )}
    </div>
  );
}

export default ProfileCertificates;
