import { useNavigate, useParams } from "react-router-dom";
import Button from "../../general/button/button";
import CardContainer from "../cardContainer";
import PurchaseInfo from "../purchaseInfo/purchaseInfo";

import classes from "./coursePurchase.module.css";
import {
  CardInfo,
  PurchaseInfoData,
  PurchaseType,
} from "../../../types/purchase";
import { useEffect, useState } from "react";
import api from "../../../config/axios";
import { BeatLoader } from "react-spinners";
import NotFound from "../../general/responses/notFound/notFound";

function CoursePurchase() {
  const navigate = useNavigate();

  const { courseId } = useParams();
  const [course, setCourse] = useState<PurchaseInfoData | null | undefined>(
    undefined
  );
  const [card, setCard] = useState<CardInfo | null>();
  const [error, setError] = useState<string | null>();

  useEffect(() => {
    api
      .get("/purchases/new/course/" + courseId)
      .then((response: any) => {
        setCourse(response.data as PurchaseInfoData);
      })
      .catch((error) => {
        if (error.response.status == 404) setCourse(null);
      });
  }, []);

  if (course === undefined) return <BeatLoader color="var(--accent-color)" />;
  if (course === null) return <NotFound>Курс не найден</NotFound>;

  return (
    <div className={classes.coursePurchaseContainer}>
      <h2 className={classes.pageTitle}>Покупка</h2>
      <PurchaseInfo
        title={course.title}
        price={course.price}
        purchaseType={PurchaseType.Course}
      />
      {error != null && <div className={classes.error}>{error}</div>}
      <form
        style={{
          display: "flex",
          flexDirection: "column",
          gap: "0.5rem",
          alignItems: "center",
        }}
        onSubmit={(event) => {
          event.preventDefault();
          // логика
        }}
      >
        <CardContainer onChange={setCard} />
        <div className={classes.purchaseFooter}>
          {card == null ? (
            <Button disabled>оплатить</Button>
          ) : (
            <Button>оплатить</Button>
          )}
          <Button
            onClick={(event: any) => {
              event.preventDefault();
              navigate(-1);
            }}
          >
            отмена
          </Button>
        </div>
      </form>
    </div>
  );
}

export default CoursePurchase;
