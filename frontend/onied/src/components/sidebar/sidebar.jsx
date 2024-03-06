import classes from "./sidebar.module.css";

function Sidebar() {
  return (
    <div className={classes.sidebar}>
      <div className={classes.title}>название курса без регистра</div>
    </div>
  );
}

export default Sidebar;
