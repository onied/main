import classes from "./tasks.module.css";
import PointBadge from "./pointBadge";
import { TaskPointsInfo } from "@onied/types/task";

type props = {
  taskTitle: string;
  pointInfo: TaskPointsInfo;
};

function TaskTitle({ taskTitle, pointInfo }: props) {
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
