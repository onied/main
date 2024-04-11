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
        <div className={classes.gridItem}>Особенности</div>
        <div className={classes.gridItem}>Бесплатный</div>
        <div className={classes.gridItem}>Базовый</div>
        {props.featureDescriptions.map((featureInfo) => (
          <>
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
          </>
        ))}
      </div>
      <div className={classes.lastColumnWrapper}>
        <div className={classes.gridItemLastColumn}>Полный</div>
        {props.featureDescriptions.map((featureInfo) => (
          <div className={classes.gridItemLastColumn}>
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
