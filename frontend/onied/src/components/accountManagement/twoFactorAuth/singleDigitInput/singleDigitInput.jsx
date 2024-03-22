import classes from "./singleDigitInput.module.css"
import PropTypes from 'prop-types';


function SingleDigitInput({ className, ...props }) {
  return (
    <>
      <input className={`${classes.singleDigitInput} ${className}`} maxLength="1" {...props} required />
    </>
  );
}

SingleDigitInput.propTypes = {
  className: PropTypes.string,
};

export default SingleDigitInput;
