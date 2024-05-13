import classes from "./profile.module.css";
import { useProfile } from "../../hooks/profile/useProfile";
import { useEffect, useState } from "react";
import SubscriptionContainer, {
  Subscription,
} from "./subscription/subscription";
import api from "../../config/axios";

function ProfileSubcriptions() {
  const [profile, _] = useProfile();
  const [subscription, setSubscription] = useState<Subscription | undefined>();

  useEffect(() => {
    setSubscription(undefined);
    api
      .get("purchases/subscriptions/active")
      .then((res) => {
        if (res.data) {
          res.data.endDate = new Date(res.data.endDate);
          setSubscription(res.data);
        }
      })
      .catch((error) => console.log(error));
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
