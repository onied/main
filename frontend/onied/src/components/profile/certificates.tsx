import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import CertificateContainer, { Certificate } from "./certificate/certificate";
import { useEffect, useState } from "react";
import api from "../../config/axios";

function ProfileCertificates() {
  const [profile, _] = useProfile();
  const [certificateList, setCertificateList] = useState<
    Array<Certificate> | undefined
  >();

  useEffect(() => {
    api
      .get("/certificates")
      .then((response) => {
        setCertificateList(response.data);
      })
      .catch((error) => console.log(error));
  }, []);

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
