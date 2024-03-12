import { useState } from "react";
import Radio from "../../general/radio/radio";
import classes from "./tasks.module.css";
import taskType from "./taskType";

function SingleAnswersTask({ task, onChange }) {
  const [value, setValue] = useState();

  const handleChange = (event) => {
    setValue(event.target.value);

    onChange({
      taskId: task.id,
      taskType: taskType.SINGLE_ANSWER,
      isDone: true,
      variantsIds: [Number(event.target.value)]
    });
  }

  return (
    <>
      {task.variants.map((variant) => {
        return (
          <div key={variant.id} className={classes.variant}>
            <Radio
              name={task.id}
              id={variant.id}
              value={variant.id}
              onChange={handleChange}
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
