import { Navigate, useParams } from "react-router-dom";
import Button from "../../general/button/button";
import CardContainer from "../cardContainer";
import PurchaseInfo from "../purchaseInfo/purchaseInfo";

import classes from "./coursePurchase.module.css";
import { CoursePurchaseInfo, PurchaseType } from "../../../types/purchase";
import { useEffect, useState } from "react";
import { useProfile } from "../../../hooks/profile/useProfile";
import api from "../../../config/axios";
import { BeatLoader } from "react-spinners";

function CoursePurchase() {
  const NotFound = <h2>Курс не найден</h2>;

  const [profile, loading] = useProfile();
  const { courseId } = useParams();
  const [course, setCourse] = useState<CoursePurchaseInfo | null | undefined>(
    undefined
  );

  useEffect(() => {
    api
      .get("/courses/" + courseId)
      .then((response: any) => {
        setCourse(response.data as CoursePurchaseInfo);
      })
      .catch((error) => {
        if (error.response.status == 404) setCourse(null);
      });
  }, []);

  if (profile == null && !loading) return <Navigate to="/login" />;
  if (loading || course === undefined) return <BeatLoader />;
  if (course === null) return NotFound;

  return (
    <div className={classes.coursePurchaseContainer}>
      <h2 className={classes.pageTitle}>Покупка</h2>
      <PurchaseInfo
        title={course.title}
        price={course.price}
        purchaseType={PurchaseType.Course}
      />
      <CardContainer onChange={(a) => {}} />
      <div className={classes.purchaseFooter}>
        <Button>оплатить</Button>
        <Button>отмена</Button>
      </div>
    </div>
  );
}

export default CoursePurchase;
