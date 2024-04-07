import classes from "../inputform/inputform.module.css";

function InputFormArea(props: any) {
  return <textarea className={classes.inputform} {...props}></textarea>;
}

export default InputFormArea;
