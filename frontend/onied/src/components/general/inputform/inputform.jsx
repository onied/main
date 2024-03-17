import classes from "./inputform.module.css";

function InputForm(props) {
  return <input type="text" className={classes.inputform} {...props}></input>;
}

export default InputForm;
