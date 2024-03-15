import classes from "./tasks.module.css";

function PointBadge({ checked, maxPoints, points }) {
  return (
    <span className={classes.pointBadge}>
      {checked ? points + "/" + maxPoints : "не проверено"}
    </span>
  );
}

export default PointBadge;
