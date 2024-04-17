import classes from "./button.module.css";

function Button({ className, ...props }) {
  return (
    <>
      <button
        className={[classes.button, className].join(" ")}
        {...props}
      ></button>
    </>
  );
}

export default Button;
