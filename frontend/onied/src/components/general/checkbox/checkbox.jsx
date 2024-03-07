import classes from "./checkbox.module.css";

function Checkbox(props) {
  return (
    <input type="checkbox" className={classes.checkbox} {...props}></input>
  );
}

export default Checkbox;
