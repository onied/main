import classes from "./subscription.module.css";
import SubscriptionHeader from "./subscriptionHeader";
import SubscriptionFeatures from "./subscriptionFeatures";
import Checkbox from "../../general/checkbox/checkbox";

export enum SubscriptionType {
  Default,
  Full,
}

export type Subscription = {
  id: number;
  title: string;
  endDate: Date;
  autoRenewalEnabled: boolean;
  coursesHighlightingEnabled: boolean;
  showingMainPageEnabled: boolean;
  adsEnabled: boolean;
  certificatesEnabled: boolean;
  activeCoursesNumber: number;
};

function SubscriptionContainer({
  subscription,
  onChange,
}: {
  subscription: Subscription;
  onChange: (subscription: Subscription) => void;
}) {
  const updateAutoRenewal = () => {
    onChange({
      ...subscription!,
      autoRenewalEnabled: !subscription.autoRenewalEnabled,
    });
  };

  return (
    <div
      className={
        subscription.coursesHighlightingEnabled
          ? classes.highlightingSubscriptionCard
          : classes.defaultSubscriptionCard
      }
    >
      <SubscriptionHeader subscription={subscription} />
      <SubscriptionFeatures
        coursesHighlightingEnabled={subscription.coursesHighlightingEnabled}
        showingMainPageEnabled={subscription.showingMainPageEnabled}
        activeCoursesNumber={subscription.activeCoursesNumber}
        adsEnabled={subscription.adsEnabled}
        certificatesEnabled={subscription.certificatesEnabled}
      />
      <div className={classes.checkboxAndTitle}>
        <Checkbox
          id="auto-renewal-checkbox"
          checked={subscription?.autoRenewalEnabled}
          onChange={updateAutoRenewal}
        />
        <label className={classes.formLabel} htmlFor="auto-renewal-checkbox">
          Автоматическое продление
        </label>
      </div>
    </div>
  );
}

export default SubscriptionContainer;
