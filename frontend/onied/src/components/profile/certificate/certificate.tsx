import classes from "./certificate.module.css";
import CertificateImage from "../../../assets/certificate.svg";
import { Link } from "react-router-dom";
import Button from "../../general/button/button";

export type Certificate = {
  courseTitle: string;
  courseId: string;
};

function CertificateContainer({ certificate }: { certificate: Certificate }) {
  return (
    <div className={classes.container}>
      <img src={CertificateImage}></img>
      <p className={classes.title}>{certificate.courseTitle}</p>
      <Button>получить</Button>
    </div>
  );
}

export default CertificateContainer;
