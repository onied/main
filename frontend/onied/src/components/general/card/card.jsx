import classes from "./card.module.css";
import PropTypes from "prop-types";

function Card({ className, ...props }) {
  return (
    <>
      <div className={`${classes.card} ${className}`} {...props}></div>
    </>
  );
}

Card.propTypes = {
  className: PropTypes.string,
};

export default Card;
