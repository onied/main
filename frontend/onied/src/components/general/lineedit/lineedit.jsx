import classes from "./lineedit.module.css";

function LineEdit(props) {
  return <input type="text" className={classes.lineedit} {...props}></input>;
}

export default LineEdit;
