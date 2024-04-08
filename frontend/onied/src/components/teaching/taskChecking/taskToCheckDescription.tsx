import classes from "./taskChecking.module.css";
import Arrow from "../../../assets/arrow.svg";
import { TaskToCheck } from "./taskChecking";

function TaskToCheckDescription(props: TaskToCheck) {
  return (
    <div className={classes.taskWrapper}>
      <div className={classes.taskName}>{props.taskTitle}</div>
      <div className={classes.moduleAndBlockName}>
        <span>{props.moduleName}</span>
        <img src={Arrow} />
        <span>{props.blockName}</span>
      </div>
    </div>
  );
}

export default TaskToCheckDescription;
