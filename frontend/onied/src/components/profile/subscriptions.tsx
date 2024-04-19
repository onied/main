import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import { useEffect, useState } from "react";
import SubscriptionContainer, {
  Subscription,
} from "./subscription/subscription";

function ProfileSubcriptions() {
  const [profile, _] = useProfile();
  const [subscriptionsList, setSubscriptionsList] = useState<
    Array<Subscription> | undefined
  >();

  const handleSubscriptionChange = (updatedSubscription: Subscription) => {
    const updatedSubscriptions = subscriptionsList?.map((subscription) => {
      if (subscription.subscriptionId === updatedSubscription.subscriptionId) {
        return updatedSubscription;
      }
      return subscription;
    });
    setSubscriptionsList(updatedSubscriptions);
  };

  useEffect(() => {
    setSubscriptionsList(undefined);
    /*setSubscriptionsList([{
      subscriptionId: 1,
      subscriptionType: 0,
      endDate: new Date("2024-04-31"),
      autoRenewalEnabled: true,
    },
    {
      subscriptionId: 2,
      subscriptionType: 1,
      endDate: new Date("2024-04-25"),
      autoRenewalEnabled: true,
    }])*/
  }, []);

  if (profile == null) return <></>;

  if (!subscriptionsList)
    return (
      <p className={classes.noCourses} style={{ margin: "40px" }}>
        У вас нет активных подписок
      </p>
    );

  return (
    <div className={classes.gridContainer}>
      {subscriptionsList.map((subscription, index) => (
        <SubscriptionContainer
          subscription={subscription}
          onChange={handleSubscriptionChange}
          key={index}
        ></SubscriptionContainer>
      ))}
    </div>
  );
}

export default ProfileSubcriptions;
