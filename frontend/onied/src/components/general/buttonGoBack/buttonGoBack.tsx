import classes from "./buttonGoBack.module.css";

function ButtonGoBack({ ...props }) {
  return (
    <>
      <button className={classes.buttonGoBack} {...props}></button>
    </>
  );
}

export default ButtonGoBack;
