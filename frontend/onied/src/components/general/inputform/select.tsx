import classes from "./select.module.css";

function Select(props: any) {
  return (
    <div className={classes.customSelect}>
      <select {...props}></select>
    </div>
  );
}

export default Select;
