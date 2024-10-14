import { TaskPointsInfo } from "@onied/types/task";
import classes from "./tasks.module.css";

function PointBadge({ checked, maxPoints, points }: TaskPointsInfo) {
  return (
    <span className={classes.pointBadge}>
      {checked ? points + "/" + maxPoints : "не проверено"}
    </span>
  );
}

export default PointBadge;
