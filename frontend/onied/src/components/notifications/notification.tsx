import classes from "./notification.module.css";
import { Notification } from "../../types/notifications";

function NotificationComponent({
  notification,
}: {
  notification: Notification;
}) {
  return (
    <div className={classes.notification}>
      <div className={classes.notificationImgContainer}>
        <img src={notification.img} />
      </div>
      <div className={classes.notificationTextInfo}>
        <h4 className={classes.notificationTitle}>{notification.title}</h4>
        <span>{notification.message}</span>
      </div>
      <span className={classes.notificationReadButton} />
    </div>
  );
}

export default NotificationComponent;
