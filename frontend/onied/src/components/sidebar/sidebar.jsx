import { CompletedIcon } from "./completedIcon";
import classes from "./sidebar.module.css";

function Sidebar() {
  return (
    <div className={classes.sidebar}>
      <div className={classes.title}>название курса без регистра</div>
      <div>
        <div className={classes.module}>1. Первый модуль</div>
        <div className={classes.block}>1.1. Первый блок</div>
        <div className={classes.selected}>1.2. Второй блок</div>
        <div className={classes.block}>
          1.3. Третий блок <CompletedIcon></CompletedIcon>
        </div>
        <div className={classes.block}>1.4. Четвертый блок</div>
      </div>
      <div>
        <div className={classes.module}>2. Второй модуль</div>
        <div className={classes.block}>2.1. Первый блок</div>
        <div className={classes.block}>2.2. Второй блок</div>
        <div className={classes.block}>2.3. Третий блок</div>
        <div className={classes.block}>2.4. Четвертый блок</div>
      </div>
    </div>
  );
}

export default Sidebar;
