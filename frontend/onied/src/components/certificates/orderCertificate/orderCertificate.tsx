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
import { useProfile } from "../../../hooks/profile/useProfile";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import api from "../../../config/axios";
import axios from "axios";

export type CertificateCourseAuthor = {
  firstName: string;
  lastName: string;
};

export type CertificateCourse = {
  title: string;
  author: CertificateCourseAuthor;
};

export type Certificate = {
  price: number;
  course: CertificateCourse;
};

function OrderCertificate() {
  const navigate = useNavigate();
  const { courseId } = useParams();
  const [profile, loading] = useProfile();
  const [certificateInfo, setCertificateInfo] = useState<
    Certificate | undefined
  >();
  const [address, setAddress] = useState<string>("");
  const [loadStatus, setLoadStatus] = useState<number>(0);
  const [error, setError] = useState<string>("");
  const mapRef = useRef<MapRef>(null);
  const id = Number(courseId);

  const validateAndOrder = () => {
    setError("");
    if (address === "") {
      setError("Укажите адрес.");
      return;
    }
    axios
      .get("https://api.mapbox.com/search/geocode/v6/forward", {
        params: {
          q: address,
          country: "ru",
          access_token: Config.MapboxApiKey,
        },
      })
      .then((response) => {
        if (response.data.type === "FeatureCollection") {
          const features = response.data.features.filter(
            (feature: any) => feature.properties.feature_type === "address"
          );
          if (features.length === 0) setError("Адрес не найден.");
          else {
            const main = features[0];
            setAddress(main.properties.full_address);
            navigate("/purchases/certificate/" + courseId, {
              state: { address: main.properties.full_address },
            });
          }
        }
      });
  };

  useEffect(() => {
    if (loading) return;
    if (isNaN(id)) {
      setLoadStatus(-1);
      return;
    }
    api
      .get("/certificates/" + id)
      .then((response) => {
        setCertificateInfo(response.data);
        setLoadStatus(200);
      })
      .catch((e) => {
        console.log(e);
        if (e.response != null) setLoadStatus(e.response.status);
        else setLoadStatus(-1);
      });
  }, [loading, id]);

  if (loadStatus == 0 || loading)
    return (
      <BeatLoader style={{ margin: "3rem" }} color="var(--accent-color)" />
    );
  if (profile === null) return <Navigate to="/login"></Navigate>;
  if (loadStatus != 200) return <Forbid>Произошла ошибка.</Forbid>;
  if (certificateInfo === undefined) return <NotFound>Курс не найден</NotFound>;

  return (
    <>
      <div className={classes.header}>
        <h1 className={classes.headerTitle}>Заказ сертификата</h1>
      </div>
      <div className={classes.printCertificateWrapper}>
        <PrintCertificate
          course={certificateInfo.course}
          profile={profile}
        ></PrintCertificate>
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
            {
              // @ts-ignore
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
                  types: "address, city",
                }}
                marker={true}
                mapboxgl={mapboxgl}
              />
            }
          </Map>
          {error ? <div className={classes.error}>{error}</div> : <></>}
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
        <Button onClick={validateAndOrder}>заказать</Button>
      </div>
    </>
  );
}

export default OrderCertificate;
