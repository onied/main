import { useState } from "react";
import { CompletedIcon } from "./completedIcon";
import classes from "./sidebar.module.css";
import BarLoader from "react-spinners/BarLoader";

function Sidebar({ hierarchy }) {
  const loaded = (attribute) => {
    return hierarchy != null ? attribute in hierarchy : false;
  };

  const renderBlock = (block, index, moduleIndex) => {
    return (
      <div className={classes.block} key={moduleIndex + "." + index + "block"}>
        {moduleIndex + 1}.{index + 1}. {block.title}
        {block.completed ? <CompletedIcon></CompletedIcon> : <></>}
      </div>
    );
  };
  const renderModule = (module, index) => {
    return (
      <div key={index + "module"}>
        <div className={classes.module}>
          {index + 1}. {module.title}
        </div>
        <div>
          {module.blocks.map((block, blockIndex) =>
            renderBlock(block, blockIndex, index)
          )}
        </div>
      </div>
    );
  };
  return (
    <div className={classes.sidebar}>
      <div className={classes.title}>
        {loaded("title") ? hierarchy.title : <></>}
      </div>
      <div>
        {loaded("modules") ? (
          hierarchy.modules.map(renderModule)
        ) : (
          <div className="d-flex justify-content-center m-5">
            <BarLoader color="var(--accent-color)" width="100%"></BarLoader>
          </div>
        )}
      </div>
    </div>
  );
}

export default Sidebar;
