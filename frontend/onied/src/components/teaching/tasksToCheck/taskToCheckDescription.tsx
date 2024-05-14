import classes from "./taskChecking.module.css";
import Arrow from "../../../assets/arrow.svg";
import { TaskToCheckInfo } from "./tasksToCheck";

function TaskToCheckDescription(props: TaskToCheckInfo) {
  return (
    <div className={classes.taskWrapper}>
      <div className={classes.taskName}>{props.title}</div>
      <div className={classes.moduleAndBlockName}>
        <span>{props.moduleTitle}</span>
        <img src={Arrow} />
        <span>{props.blockTitle}</span>
      </div>
    </div>
  );
}

export default TaskToCheckDescription;
