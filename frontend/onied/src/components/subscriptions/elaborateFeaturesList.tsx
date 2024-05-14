import Checkmark from "../../assets/checkmark.svg";
import Cross from "../../assets/cross.svg";
import classes from "./elaborateFeatures.module.css";
import { SubscriptionFeatureInfo } from "../../pages/subscriptions/subscriptionsPreview";

function ElaborateFeaturesList(props: {
  featureDescriptions: Array<SubscriptionFeatureInfo>;
  subscriptionsTitles: Array<string>;
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
          {props.subscriptionsTitles[0]}
        </div>
        <div
          className={`${classes.gridItem} ${classes.defaultSubscriptionTitle}`}
        >
          {props.subscriptionsTitles[1]}
        </div>
        {props.featureDescriptions.map((featureInfo, index) => (
          <div key={index} style={{ display: "contents" }}>
            <div className={classes.gridItem}>
              {featureInfo.featureDescription}
            </div>
            <div className={classes.gridItem}>
              {typeof featureInfo.free === "boolean" ? (
                <img src={featureInfo.free ? Checkmark : Cross} />
              ) : featureInfo.free == "-1" ? (
                <span style={{ fontSize: "1.5rem" }}>∞</span>
              ) : (
                <span style={{ fontSize: "1.5rem" }}>{featureInfo.free}</span>
              )}
            </div>
            <div className={classes.gridItem}>
              {typeof featureInfo.default === "boolean" ? (
                <img src={featureInfo.default ? Checkmark : Cross} />
              ) : featureInfo.default == "-1" ? (
                <span style={{ fontSize: "1.5rem" }}>∞</span>
              ) : (
                <span style={{ fontSize: "1.5rem" }}>
                  {featureInfo.default}
                </span>
              )}
            </div>
          </div>
        ))}
      </div>
      <div className={classes.lastColumnWrapper}>
        <div
          className={`${classes.gridItemLastColumn} ${classes.fullSubscriptionTitle}`}
        >
          {props.subscriptionsTitles[2]}
        </div>
        {props.featureDescriptions.map((featureInfo, index) => (
          <div className={classes.gridItemLastColumn} key={index}>
            {typeof featureInfo.full === "boolean" ? (
              <img src={featureInfo.full ? Checkmark : Cross} />
            ) : featureInfo.full == "-1" ? (
              <span style={{ fontSize: "1.5rem" }}>∞</span>
            ) : (
              <span style={{ fontSize: "1.5rem" }}>{featureInfo.full}</span>
            )}
          </div>
        ))}
      </div>
    </div>
  );
}

export default ElaborateFeaturesList;
