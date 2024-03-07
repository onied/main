import { useState } from "react";
import Radio from "../../general/radio/radio";
import classes from "./tasks.module.css";

function SingleAnswersTask({ task }) {
  const [value, setValue] = useState();
  return (
    <>
      {task.variants.map((variant) => {
        return (
          <div key={variant.id} className={classes.variant}>
            <Radio
              name={task.id}
              id={variant.id}
              value={variant.id}
              onChange={(event) => setValue(event.target.value)}
              checked={value == variant.id ? "t" : ""}
            ></Radio>
            <label htmlFor={variant.id} className={classes.variantLabel}>
              {variant.description}
            </label>
          </div>
        );
      })}
    </>
  );
}

export default SingleAnswersTask;
