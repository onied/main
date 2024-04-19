import { useEffect, useState } from "react";
import { Notification } from "../../../types/notifications";
import NotificationComponent from "../notificationComponent";

import bellLogo from "../../../assets/bell.svg";
import bellActiveLogo from "../../../assets/bellActive.svg";
import classes from "./notificationContainer.module.css";

function NotificationContainer() {
  const [unread, setUnread] = useState<boolean>(false);
  const [showDropdown, setShowDropdown] = useState<boolean>(false);
  const [notifications, setNotifications] = useState<Notification[]>([
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
      isRead: false,
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
      isRead: false,
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
      isRead: true,
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
      isRead: true,
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
      isRead: true,
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
      isRead: true,
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
      isRead: true,
    },
  ]);

  useEffect(() => {
    setUnread(notifications.some((n) => !n.isRead));
  }, [notifications]);

  const updateNotification = (index: number, notification: Notification) => {
    var newNotifications = [...notifications];
    newNotifications[index] = notification;
    setNotifications(newNotifications);
  };

  return (
    <div className={classes.notificationsWrapper}>
      <div className={classes.notificationsButtonContainer}>
        <img
          src={unread ? bellActiveLogo : bellLogo}
          onClick={() => setShowDropdown((value) => !value)}
        />
      </div>
      <div
        className={[
          classes.notidicationsDropdown,
          showDropdown ? classes.notidicationsDropdownOpen : "",
        ]
          .join(" ")
          .trim()}
      >
        <div className={classes.notificationsList}>
          {notifications.map((value, index) => (
            <NotificationComponent
              key={"notification_" + index}
              notification={value}
              onRead={(notification: Notification) => {
                updateNotification(index, notification);
              }}
            />
          ))}
        </div>
      </div>
    </div>
  );
}

export default NotificationContainer;
