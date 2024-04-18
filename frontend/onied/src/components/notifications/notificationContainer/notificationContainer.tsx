import { Notification } from "../../../types/notifications";
import NotificationComponent from "../notificationComponent";
import classes from "./notificationContainer.module.css";

function NotificationContainer({
  notifications,
}: {
  notifications: Notification[];
}) {
  return (
    <div className={classes.notificationsContainer}>
      {notifications.map((value, index) => (
        <NotificationComponent
          key={"notification_" + index}
          notification={value}
        />
      ))}
    </div>
  );
}

export default NotificationContainer;
