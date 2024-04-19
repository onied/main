import { useState } from "react";
import { Notification } from "../../../types/notifications";
import NotificationComponent from "../notificationComponent";

import bellLogo from "../../../assets/bell.svg";
import bellActiveLogo from "../../../assets/bellActive.svg";
import classes from "./notificationContainer.module.css";

function NotificationContainer() {
  const [newNotifications, setNewNotifications] = useState<boolean>(false);
  const [showDropdown, setShowDropdown] = useState<boolean>(false);
  const [notifications, setNotifications] = useState<Notification[]>([
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
    },
    {
      id: 1,
      title: "sdfsdfsf ddsfsdfs dfdfddsffsd",
      img: "https://images-prod.dazeddigital.com/1090/134-1-1090-726/azure/dazed-prod/1310/5/1315594.jpeg",
      message: "z fdgfgfdsgfsdfaasd dsfsdfd",
    },
  ]);

  return (
    <div className={classes.notificationsWrapper}>
      <div className={classes.notificationsButtonContainer}>
        <img
          src={newNotifications ? bellActiveLogo : bellLogo}
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
            />
          ))}
        </div>
      </div>
    </div>
  );
}

export default NotificationContainer;
