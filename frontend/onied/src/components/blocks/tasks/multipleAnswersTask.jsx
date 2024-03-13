import { useState } from "react";
import Checkbox from "../../general/checkbox/checkbox";
import classes from "./tasks.module.css";
import taskType from "./taskType";

function MultipleAnswersTask({ task, onChange }) {
  const [values, setValues] = useState([]);

  const handleChange = (event) => {
    const currentVariantId = Number(event.target.value);

    const newValues = !values.includes(currentVariantId)
      ? [...values, currentVariantId]
      : values.filter((v) => v != currentVariantId);
    setValues(newValues);

    onChange({
      taskId: task.id,
      isDone: true,
      variantsIds: newValues,
    });
  }

  return (
    <>
      {task.variants.map((variant) => {
        return (
          <div key={variant.id} className={classes.variant}>
            <Checkbox
              name={task.id}
              id={variant.id}
              value={variant.id}
              onChange={handleChange}
              checked={values.includes(variant.id) ? "t" : ""}
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
