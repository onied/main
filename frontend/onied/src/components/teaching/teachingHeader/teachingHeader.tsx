import { useEffect, useState } from "react";
import classes from "./teachingHeader.module.css";
import { Link, useLocation } from "react-router-dom";
import Button from "../../general/button/button";

type Tab = {
  title: string;
  url: string;
};

function TeachingHeader() {
  const location = useLocation();
  const [currentTabUrl, setCurrentTabUrl] = useState("/");
  const tabs: Array<Tab> = [
    { title: "Авторство", url: "" },
    { title: "Модерация", url: "moderating" },
    { title: "Проверка заданий", url: "check" },
  ];
  const renderBlock = (tab: Tab, index: number) => {
    if (currentTabUrl == tab.url)
      return (
        <span className={classes.active} key={index}>
          {tab.title}
        </span>
      );
    return (
      <Link className={classes.regular} key={index} to={`/teaching/${tab.url}`}>
        {tab.title}
      </Link>
    );
  };

  useEffect(() => {
    setCurrentTabUrl(
      location.pathname
        .replace("/teaching", "")
        .replace(/^\//, "")
        .replace(/\/$/, "")
    );
  }, [location.pathname]);

  return (
    <div className={classes.header}>
      <span>{tabs.map(renderBlock)}</span>
      <Link to="/course/create">
        <Button className={classes.button}>создать курс</Button>
      </Link>
    </div>
  );
}

export default TeachingHeader;
