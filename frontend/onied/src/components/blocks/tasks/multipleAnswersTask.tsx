import { useState } from "react";
import Checkbox from "../../general/checkbox/checkbox";
import classes from "./tasks.module.css";
import { Task, TaskType, UserInputRequest } from "@onied/types/task";

type props = {
  task: Task;
  onChange: (request: UserInputRequest) => void;
};

function MultipleAnswersTask({ task, onChange }: props) {
  const [values, setValues] = useState<number[]>([]);

  const handleChange = (event: any) => {
    const currentVariantId = Number(event.target.value);

    const newValues = !values.includes(currentVariantId)
      ? [...values, currentVariantId]
      : values.filter((v) => v != currentVariantId);
    setValues(newValues);

    onChange({
      taskId: task.id,
      taskType: TaskType.MultipleAnswers,
      isDone: true,
      variantsIds: newValues,
    });
  }

  return (
    <>
      {task.variants?.map((variant) => {
        return (
          <div key={variant.id} className={classes.variant}>
            <Checkbox
              name={task.id}
              id={variant.id}
              value={variant.id}
              onChange={handleChange}
              checked={values.includes(variant.id) ? "t" : ""}
            ></Checkbox>
            <label htmlFor={variant.id.toString()} className={classes.variantLabel}>
              {variant.description}
            </label>
          </div>
        );
      })}
    </>
  );
}

export default MultipleAnswersTask;
