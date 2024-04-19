import classes from "./notification.module.css";
import { Notification } from "../../../types/notifications";

function NotificationComponent({
  notification,
  onRead,
}: {
  notification: Notification;
  onRead: (notification: Notification) => void;
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
      {!notification.isRead && (
        <span
          className={classes.notificationReadButton}
          onClick={() => onRead({ ...notification, isRead: true })}
        />
      )}
    </div>
  );
}

export default NotificationComponent;
