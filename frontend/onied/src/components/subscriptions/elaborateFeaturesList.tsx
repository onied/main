import Checkmark from "../../assets/checkmark.svg";
import Cross from "../../assets/cross.svg";
import classes from "./elaborateFeatures.module.css";
import { SubscriptionFeatureInfo } from "../../pages/subscriptions/subscriptionsPreview";

function ElaborateFeaturesList(props: {
  featureDescriptions: Array<SubscriptionFeatureInfo>;
}) {
  return (
    <div className={classes.outerGrid}>
      <div className={classes.innerGrid}>
        <div
          className={`${classes.gridItem} ${classes.featureDescriptionsTitle}`}
        >
          Особенности
        </div>
        <div className={`${classes.gridItem} ${classes.freeSubscriptionTitle}`}>
          Бесплатный
        </div>
        <div
          className={`${classes.gridItem} ${classes.defaultSubscriptionTitle}`}
        >
          Базовый
        </div>
        {props.featureDescriptions.map((featureInfo, index) => (
          <div key={index} style={{ display: "contents" }}>
            <div className={classes.gridItem}>
              {featureInfo.featureDescription}
            </div>
            <div className={classes.gridItem}>
              {typeof featureInfo.free === "boolean" ? (
                <img src={featureInfo.free ? Checkmark : Cross} />
              ) : (
                featureInfo.free
              )}
            </div>
            <div className={classes.gridItem}>
              {typeof featureInfo.default === "boolean" ? (
                <img src={featureInfo.default ? Checkmark : Cross} />
              ) : (
                featureInfo.default
              )}
            </div>
          </div>
        ))}
      </div>
      <div className={classes.lastColumnWrapper}>
        <div
          className={`${classes.gridItemLastColumn} ${classes.fullSubscriptionTitle}`}
        >
          Полный
        </div>
        {props.featureDescriptions.map((featureInfo, index) => (
          <div className={classes.gridItemLastColumn} key={index}>
            {typeof featureInfo.full === "boolean" ? (
              <img src={featureInfo.full ? Checkmark : Cross} />
            ) : (
              featureInfo.full
            )}
          </div>
        ))}
      </div>
    </div>
  );
}

export default ElaborateFeaturesList;
