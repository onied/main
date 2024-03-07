import { useState } from "react";
import Checkbox from "../../general/checkbox/checkbox";
import classes from "./tasks.module.css";

function MultipleAnswersTask({ task }) {
  const [value, setValue] = useState([]);
  return (
    <>
      {task.variants.map((variant) => {
        return (
          <div key={variant.id} className={classes.variant}>
            <Checkbox
              name={task.id}
              id={variant.id}
              value={variant.id}
              onChange={(event) => {
                !value.includes(variant.id)
                  ? setValue([...value, variant.id])
                  : setValue(value.filter((v) => v != variant.id));
              }}
              checked={value.includes(variant.id) ? "t" : ""}
            ></Checkbox>
            <label htmlFor={variant.id} className={classes.variantLabel}>
              {variant.description}
            </label>
          </div>
        );
      })}
    </>
  );
}

export default MultipleAnswersTask;
