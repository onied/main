import { useEffect, useState } from "react";
import { CompletedIcon } from "./completedIcon";
import classes from "./sidebar.module.css";
import BarLoader from "react-spinners/BarLoader";
import { Link, useLocation } from "react-router-dom";
import { useProfile } from "../../hooks/profile/useProfile";

type Tab = {
  title: string;
  url: string;
};

function ProfileSidebar() {
  const location = useLocation();
  const [currentTabUrl, setCurrentTabUrl] = useState("/");
  const profile = useProfile();
  const tabs: Array<Tab> = [
    { title: "Мои данные", url: "" },
    { title: "Мои курсы", url: "courses" },
    { title: "Мои сертификаты", url: "certificates" },
  ];
  const renderBlock = (tab: Tab, index: number) => {
    if (currentTabUrl == tab.url)
      return (
        <div className={classes.module} key={index}>
          {tab.title}
        </div>
      );
    return (
      <Link className={classes.block} key={index} to={`/profile/${tab.url}`}>
        {tab.title}
      </Link>
    );
  };

  useEffect(() => {
    setCurrentTabUrl(
      location.pathname
        .replace("/profile", "")
        .replace(/^\//, "")
        .replace(/\/$/, "")
    );
  }, [location.pathname]);

  return (
    <div className={classes.sidebar}>
      <div>
        {profile != null ? (
          tabs.map(renderBlock)
        ) : (
          <div className="d-flex justify-content-center m-5">
            <BarLoader color="var(--accent-color)" width="100%"></BarLoader>
          </div>
        )}
      </div>
    </div>
  );
}

export default ProfileSidebar;
