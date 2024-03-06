import { useState } from "react";
import { CompletedIcon } from "./completedIcon";
import classes from "./sidebar.module.css";

function Sidebar() {
  const [hierarchy, setHierarchy] = useState({
    title: "нАзвАниЕ курсА бЕз рЕгистрА",
    modules: [
      {
        title: "Первый модуль",
        blocks: [
          { title: "Первый блок", completed: false },
          { title: "Второй блок", completed: false },
          { title: "Третий блок", completed: true },
          { title: "Четвертый блок", completed: false },
        ],
      },
      {
        title: "Второй модуль",
        blocks: [
          { title: "Первый блок", completed: false },
          { title: "Второй блок", completed: false },
          { title: "Третий блок", completed: false },
          { title: "Четвертый блок", completed: false },
        ],
      },
    ],
  });
  const renderBlock = (block, index, moduleIndex) => {
    return (
      <div className={classes.block}>
        {moduleIndex + 1}.{index + 1}. {block.title}
        {block.completed ? <CompletedIcon></CompletedIcon> : <></>}
      </div>
    );
  };
  const renderModule = (module, index) => {
    return (
      <>
        <div className={classes.module}>
          {index + 1}. {module.title}
        </div>
        <div>
          {module.blocks.map((block, blockIndex) =>
            renderBlock(block, blockIndex, index)
          )}
        </div>
      </>
    );
  };
  return (
    <div className={classes.sidebar}>
      <div className={classes.title}>{hierarchy.title.toLowerCase()}</div>
      <div>{hierarchy.modules.map(renderModule)}</div>
    </div>
  );
}

export default Sidebar;
