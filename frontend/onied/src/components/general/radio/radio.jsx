import classes from "./radio.module.css";

function Radio(props) {
  return <input type="radio" className={classes.radio} {...props}></input>;
}

export default Radio;
