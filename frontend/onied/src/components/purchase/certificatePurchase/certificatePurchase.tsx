import { useLocation, useNavigate, useParams } from "react-router-dom";
import Button from "../../general/button/button";
import CardContainer from "../cardContainer";
import PurchaseInfo from "../purchaseInfo/purchaseInfo";

import classes from "./certificatePurchase.module.css";
import {
  CardInfo,
  PurchaseInfoData,
  PurchaseType,
} from "../../../types/purchase";
import { useEffect, useState } from "react";
import api from "../../../config/axios";
import NotFound from "../../general/responses/notFound/notFound";
import CustomBeatLoader from "../../general/customBeatLoader";

function CertificatePurchase() {
  const navigate = useNavigate();

  const { courseId } = useParams();
  const [loading, setLoading] = useState<boolean>();
  const [purchaseInfo, setPurchaseInfo] = useState<PurchaseInfoData | null>(
    null
  );
  const [card, setCard] = useState<CardInfo | null>();
  const [error, setError] = useState<string | null>();

  const { state } = useLocation();
  const address = state?.address;

  useEffect(() => {
    if (address == null) {
      navigate(-1);
      return;
    }
    setLoading(true);
    api
      .get("/purchases/new/certificate?courseId=" + courseId)
      .then((response: any) => {
        const purchaseInfo = response.data as PurchaseInfoData;
        setPurchaseInfo(purchaseInfo);
        setLoading(false);
      })
      .catch((error) => {
        if (error.response.status == 404) setPurchaseInfo(null);
        setLoading(false);
      });
  }, []);

  if (address == null) return <></>;
  if (loading) return <CustomBeatLoader />;
  if (error == null && purchaseInfo === null)
    return <NotFound>Курс не найден</NotFound>;

  return (
    <div className={classes.certificatePurchaseContainer}>
      <h2 className={classes.pageTitle}>Покупка</h2>
      <PurchaseInfo
        title={purchaseInfo!.title}
        price={purchaseInfo!.price}
        purchaseType={PurchaseType.Certificate}
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
            purchaseType: PurchaseType.Certificate,
            price: purchaseInfo!.price,
            cardInfo: card!,
            courseId: courseId,
          };
          setLoading(true);
          api
            .post("/purchases/new/certificate", purchase)
            .then(() => {
              api
                .post("/certificates/" + courseId + "/order", {
                  address: address,
                })
                .then((response) => {
                  alert(
                    "Заказ был успешно совершен. ID заказа: " +
                      response.data.orderId
                  );
                })
                .catch(console.log)
                .finally(() => {
                  navigate(-1);
                });
            })
            .catch((error) => {
              if (error.response.status == 400)
                setError("Возникла ошибка при валидации");
              else if (error.response.status == 403)
                setError("Вы не можете купить данный сертификат");
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

export default CertificatePurchase;
