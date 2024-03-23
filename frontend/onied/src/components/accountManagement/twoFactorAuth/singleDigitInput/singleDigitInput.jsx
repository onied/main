import classes from "./singleDigitInput.module.css"
import PropTypes from 'prop-types';


function SingleDigitInput({ className, innerRef, ...props }) {
  return (
    <>
      <input className={`${classes.singleDigitInput} ${className}`} ref={innerRef} maxLength="1" inputMode={"numeric"} {...props} required on/>
    </>
  );
}

SingleDigitInput.propTypes = {
  className: PropTypes.string,
  innerRef: PropTypes.oneOfType([
    PropTypes.func,
    PropTypes.shape({ current: PropTypes.instanceOf(Element) })
  ])
};

export default SingleDigitInput;
