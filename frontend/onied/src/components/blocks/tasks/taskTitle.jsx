import PointBadge from "./pointBadge";
import classes from "./tasks.module.css";

function TaskTitle({ taskTitle, pointInfo }) {
  return (
    <>
      <h2 className={classes.taskTitle}>
        {taskTitle}
        <PointBadge
          maxPoints={pointInfo.maxPoints}
          points={pointInfo.points}
          checked={pointInfo.checked}
        ></PointBadge>
      </h2>
    </>
  );
}

export default TaskTitle;
