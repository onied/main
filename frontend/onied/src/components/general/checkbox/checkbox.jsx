import classes from "./checkbox.module.css";
import PropTypes from "prop-types";

function Checkbox({ className = undefined, variant = "regular", ...props }) {
  return (
    <input
      type="checkbox"
      className={[
        variant === "formal" ? classes.formal : classes.regular,
        className,
      ]
        .join(" ")
        .trim()}
      {...props}
    ></input>
  );
}

Checkbox.propTypes = {
  className: PropTypes.string,
  variant: PropTypes.oneOf(["formal", "regular"]),
};

export default Checkbox;
