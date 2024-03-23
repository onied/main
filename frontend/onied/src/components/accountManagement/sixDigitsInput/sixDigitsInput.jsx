import SingleDigitInput from "../twoFactorAuth/singleDigitInput/singleDigitInput";
import { useState, useRef } from "react";

import classes from "./sixDigitsInput.module.css";

function SixDigitsInput({ onChange }) {
  const [digits, setDigits] = useState(["", "", "", "", "", ""]);
  const inputsRefs = useRef([]);

  const handleChange = (index, event) => {
    const value = event.target.value;
    if (value === "") {
      handleBackspace(index);
    } else if (/^\d$/.test(value)) {
      handleInput(index, value);
    }
  };

  const handleInput = (index, value) => {
    setDigits((prevDigits) => {
      const newDigits = [...prevDigits];
      newDigits[index] = value;

      onChange(newDigits.join(""));

      return newDigits;
    });
    const nextIndex = index + 1;
    if (nextIndex < inputsRefs.current.length) {
      inputsRefs.current[nextIndex].focus();
    }
  };

  const handleBackspace = (index) => {
    setDigits((prevDigits) => {
      const newDigits = [...prevDigits];
      newDigits[index] = "";

      onChange(newDigits.join(""));

      return newDigits;
    });
    const prevIndex = index - 1;
    if (prevIndex >= 0) {
      inputsRefs.current[prevIndex].focus();
    }
  };

  return (
    <div className={classes.inputDigits}>
      {[...Array(6).keys()].map((index) => (
        <SingleDigitInput
          key={index}
          id={`input-${index}`}
          innerRef={(input) => (inputsRefs.current[index] = input)}
          onChange={(event) => handleChange(index, event)}
          className={classes.singleDigitInput}
          value={digits[index]}
        />
      ))}
    </div>
  );
}

export default SixDigitsInput;
