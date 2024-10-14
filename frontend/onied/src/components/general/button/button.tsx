import classes from "./button.module.css";

function Button({ ...props }) {
  return (
    <>
      <button
        className={[classes.button, props.className ?? ""].join(" ")}
        {...props}
      ></button>
    </>
  );
}

export default Button;
