import { CompletedIcon } from "./completedIcon";
import classes from "./sidebar.module.css";
import BarLoader from "react-spinners/BarLoader";
import { Link } from "react-router-dom";
import { useAppSelector } from "../../hooks";

function CoursesSidebar({ currentBlock }) {
  const hierarchyState = useAppSelector((state) => state.hierarchy);

  const hierarchy = hierarchyState.hierarchy;

  const loaded = (attribute) => {
    return hierarchy != null ? attribute in hierarchy : false;
  };

  const renderBlock = (block, index, moduleIndex) => {
    if (currentBlock == hierarchy.modules[moduleIndex].blocks[index].id)
      return (
        <div
          className={classes.selected}
          key={moduleIndex + "." + index + "block"}
        >
          {moduleIndex + 1}.{index + 1}. {block.title}
          {block.completed ? <CompletedIcon></CompletedIcon> : <></>}
        </div>
      );
    return (
      <Link
        className={classes.block}
        key={moduleIndex + "." + index + "block"}
        to={`/course/${hierarchy.id}/learn/${hierarchy.modules[moduleIndex].blocks[index].id}/`}
        onClick={() => {
          hierarchy.modules[moduleIndex].blocks[index].completed = true;
        }}
      >
        {moduleIndex + 1}.{index + 1}. {block.title}
        {block.completed ? <CompletedIcon></CompletedIcon> : <></>}
      </Link>
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

export default CoursesSidebar;
