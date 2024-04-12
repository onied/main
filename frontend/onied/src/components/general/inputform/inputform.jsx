import classes from "./inputform.module.css";

function InputForm(props) {
  return (
    <input
      type="text"
      {...props}
      className={props.className + " " + classes.inputform}
    ></input>
  );
}

export default InputForm;
