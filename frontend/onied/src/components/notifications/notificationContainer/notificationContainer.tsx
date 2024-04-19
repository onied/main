import { useState } from "react";
import { Notification } from "../../../types/notifications";
import Button from "../../general/button/button";
import NotificationComponent from "../notificationComponent";
import classes from "./notificationContainer.module.css";

function NotificationContainer() {
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
        <Button onClick={() => setShowDropdown((value) => !value)}>
          уведомления
        </Button>
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
