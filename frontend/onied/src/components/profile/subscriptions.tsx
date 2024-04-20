import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import { useEffect, useState } from "react";
import SubscriptionContainer, {
  Subscription,
} from "./subscription/subscription";

function ProfileSubcriptions() {
  const [profile, _] = useProfile();
  const [subscription, setSubscription] = useState<Subscription | undefined>();

  useEffect(() => {
    setSubscription(undefined);
    setSubscription({
      id: 1,
      title: "базовая",
      endDate: new Date("2024-04-31"),
      autoRenewalEnabled: true,
      coursesHighlightingEnabled: false,
      adsEnabled: false,
      certificatesEnabled: false,
      activeCoursesNumber: 3,
    });
    setSubscription({
      id: 2,
      title: "Полная",
      endDate: new Date("2024-04-25"),
      autoRenewalEnabled: true,
      coursesHighlightingEnabled: true,
      adsEnabled: true,
      certificatesEnabled: true,
      activeCoursesNumber: -1,
    });
  }, []);

  if (profile == null) return <></>;

  if (!subscription)
    return (
      <p className={classes.noCourses} style={{ margin: "40px" }}>
        У вас нет активных подписок
      </p>
    );

  return (
    <div className={classes.containerSubscription}>
      <SubscriptionContainer
        subscription={subscription}
        onChange={setSubscription}
      ></SubscriptionContainer>
    </div>
  );
}

export default ProfileSubcriptions;
