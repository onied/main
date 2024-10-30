import { useEffect, useState } from "react";
import { Notification } from "@onied/types/notifications";
import NotificationComponent from "../notificationComponent";
import useSignalR from "@onied/hooks/signalr";
import Config from "@onied/config/config";
import api from "@onied/config/axios";

import oniedLogo from "@onied/assets/logo.svg";
import bellLogo from "@onied/assets/bell.svg";
import bellActiveLogo from "@onied/assets/bellActive.svg";
import classes from "./notificationContainer.module.css";

function NotificationContainer() {
  const { connection } = useSignalR(
    Config.BaseURL.replace(/\/$/, "") + "/notifications/hub"
  );

  const [unread, setUnread] = useState<boolean>(false);
  const [showDropdown, setShowDropdown] = useState<boolean>(false);
  const [notifications, setNotifications] = useState<Notification[]>([]);

  useEffect(() => {
    api
      .get("notifications")
      .then((response: any) => {
        if (response.data.length == 0) {
          setNotifications([
            {
              id: Number.NaN,
              img: oniedLogo,
              title: "Уведомлений нет",
              message: "",
              isRead: true,
            },
          ]);
        } else {
          setNotifications(response.data);
        }
      })
      .catch(() => {
        console.log("error occurred while loading notifications");
      });
  }, [showDropdown]);

  useEffect(() => {
    if (!connection) return;

    connection.on("Receive", (message: Notification) => {
      setNotifications([message, ...notifications]);
      console.log("notification from the server", message);
    });

    return () => connection.off("Receive");
  }, [connection]);

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
          alt="уведомления"
        />
      </div>
      <div
        className={[
          classes.notificationsDropdown,
          showDropdown ? classes.notificationsDropdownOpen : "",
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
                connection?.send("UpdateRead", value.id);
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
