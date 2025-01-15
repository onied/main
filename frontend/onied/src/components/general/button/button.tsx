import classes from "./button.module.css";

function Button({ ...props }) {
  return (
    <>
      <button
        {...props}
        className={[classes.button, props.className ?? ""].join(" ")}
      ></button>
    </>
  );
}

export default Button;
