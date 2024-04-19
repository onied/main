import classes from "./subscription.module.css";
import SubscriptionHeader from "./subscriptionHeader";
import SubscriptionFeatures from "../../subscriptions/subscriptionCards/subscriptionFeatures";
import Checkbox from "../../general/checkbox/checkbox";

export enum SubscriptionType {
  Default,
  Full,
}

export type Subscription = {
  subscriptionId: number;
  subscriptionType: SubscriptionType;
  endDate: Date;
  autoRenewalEnabled: boolean;
};

function SubscriptionContainer({
  subscription,
  onChange,
}: {
  subscription: Subscription;
  onChange: (subscription: Subscription) => void;
}) {
  const subscriptionTypeMapInClassName = {
    0: classes.defaultSubscriptionCard,
    1: classes.fullSubscriptionCard,
  };

  const subscriptionTypeMapInFeatures = {
    0: ["3 активных платных курса"],
    1: [
      "Реклама в рассылке",
      "Выдача сертификатов",
      "3 активных платных курса",
      "Показ на главной странице",
      "Визуальное выделение курсов",
    ],
  };

  const updateAutoRenewal = () => {
    onChange({
      ...subscription!,
      autoRenewalEnabled: !subscription.autoRenewalEnabled,
    });
  };

  return (
    <div
      className={subscriptionTypeMapInClassName[subscription.subscriptionType]}
    >
      <SubscriptionHeader
        subscriptionType={subscription.subscriptionType}
        endDate={subscription.endDate}
      />
      <SubscriptionFeatures
        features={subscriptionTypeMapInFeatures[subscription.subscriptionType]}
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
