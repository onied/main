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
import NotFound from "../../general/responses/notFound/notFound";
import CustomBeatLoader from "../../general/customBeatLoader";

function CoursePurchase() {
  const navigate = useNavigate();

  const { courseId } = useParams();
  const [loading, setLoading] = useState<boolean>();
  const [purchaseInfo, setPurchaseInfo] = useState<PurchaseInfoData | null>(
    null
  );
  const [card, setCard] = useState<CardInfo | null>();
  const [error, setError] = useState<string | null>();

  useEffect(() => {
    setLoading(true);
    api
      .get("/purchases/new/course?courseId=" + courseId)
      .then((response: any) => {
        setPurchaseInfo(response.data as PurchaseInfoData);
      })
      .catch((error) => {
        if (error.response.status == 404) setPurchaseInfo(null);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  if (loading) return <CustomBeatLoader />;
  if (error == null && purchaseInfo === null)
    return <NotFound>Курс не найден</NotFound>;

  return (
    <div className={classes.coursePurchaseContainer}>
      <h2 className={classes.pageTitle}>Покупка</h2>
      <PurchaseInfo
        title={purchaseInfo!.title}
        price={purchaseInfo!.price}
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
          const purchase = {
            purchaseType: PurchaseType.Course,
            price: purchaseInfo!.price,
            cardInfo: card!,
            courseId: courseId,
          };
          setLoading(true);
          api
            .post("/purchases/new/course", purchase)
            .then(() => navigate(-1))
            .catch((error) => {
              if (error.response.status == 400)
                setError("Возникла ошибка при валидации");
              else if (error.response.status == 403)
                setError("Вы не можете купить данный курс");
              else if (error.response.status >= 500)
                setError("Возникла ошибка на сервере");
              setLoading(false);
            });
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
