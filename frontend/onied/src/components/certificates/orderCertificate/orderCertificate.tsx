import { useEffect, useRef, useState } from "react";
import Button from "../../general/button/button";
import classes from "./orderCertificate.module.css";
import Map from "react-map-gl";
import type { MapRef } from "react-map-gl";
import "mapbox-gl/dist/mapbox-gl.css";
import Config from "../../../config/config";
import { SearchBox } from "@mapbox/search-js-react";
import mapboxgl from "mapbox-gl";
import PrintCertificate from "../printCertificate/printCertificate";
import NotFound from "../../general/responses/notFound/notFound";
import { BeatLoader } from "react-spinners";
import Forbid from "../../general/responses/forbid/forbid";

type CertificateCourseAuthor = {
  firstName: string;
  lastName: string;
};

type CertificateCourse = {
  title: string;
  author: CertificateCourseAuthor;
};

type Certificate = {
  price: number;
  course: CertificateCourse;
};

function OrderCertificate() {
  const [certificateInfo, setCertificateInfo] = useState<
    Certificate | undefined
  >();
  const [address, setAddress] = useState<string>("");
  const [loadStatus, setLoadStatus] = useState<number>(0);
  const mapRef = useRef<MapRef>(null);

  useEffect(() => {
    setTimeout(() => {
      setLoadStatus(200);
      setCertificateInfo({
        price: 1000,
        course: {
          title: "Создание голограм на ноутбуке",
          author: {
            firstName: "Василий",
            lastName: "Теркин",
          },
        },
      });
    }, 500);
  });

  if (loadStatus == 0)
    return (
      <BeatLoader style={{ margin: "3rem" }} color="var(--accent-color)" />
    );
  if (loadStatus != 200) return <Forbid>Произошла ошибка.</Forbid>;
  if (certificateInfo === undefined) return <NotFound>Курс не найден</NotFound>;

  return (
    <>
      <div className={classes.header}>
        <h1 className={classes.headerTitle}>Заказ сертификата</h1>
      </div>
      <div className={classes.printCertificateWrapper}>
        <PrintCertificate></PrintCertificate>
      </div>
      <div className={classes.orderContainer}>
        <h3 className={classes.h3}>Заказать на адрес:</h3>
        <div className={classes.map}>
          <Map
            mapboxAccessToken={Config.MapboxApiKey}
            initialViewState={{
              longitude: 37.618423,
              latitude: 55.751244,
              zoom: 8,
            }}
            style={{ width: "100%", aspectRatio: "16 / 9", padding: "2rem" }}
            mapStyle="mapbox://styles/mapbox/streets-v9"
            ref={mapRef}
          >
            <SearchBox
              accessToken={Config.MapboxApiKey}
              value={address}
              onChange={setAddress}
              map={mapRef.current?.getMap()}
              onRetrieve={(resp) => {
                const feature = resp.features[0];
                if (feature == null) return;
                setAddress(feature.properties.full_address);
              }}
              options={{
                country: "RU",
                language: "ru",
                types: "address",
              }}
              marker={true}
              mapboxgl={mapboxgl}
            />
          </Map>
        </div>
      </div>
      <div className={classes.footer}>
        <h3 className={classes.h3}>
          Цена печати и доставки по России:{" "}
          {certificateInfo.price
            .toString()
            .replace(/\B(?=(\d{3})+(?!\d))/g, " ")}{" "}
          ₽
        </h3>
        <Button>заказать</Button>
      </div>
    </>
  );
}

export default OrderCertificate;
