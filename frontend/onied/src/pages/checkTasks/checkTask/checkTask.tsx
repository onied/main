import classes from "./checkTask.module.css";
import CheckTaskComponent from "../../../components/teaching/checkTask/checkTask";

function CheckTask() {
  return (
    <div className={classes.pageContainer}>
      <CheckTaskComponent></CheckTaskComponent>
    </div>
  );
}

export default CheckTask;
