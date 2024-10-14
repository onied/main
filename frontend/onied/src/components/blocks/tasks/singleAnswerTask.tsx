import { useState } from "react";
import Radio from "../../general/radio/radio";
import classes from "./tasks.module.css";
import { Task, TaskType, UserInputRequest } from "@onied/types/task";

type props = {
  task: Task;
  onChange: (request: UserInputRequest) => void;
};

function SingleAnswersTask({ task, onChange }: props) {
  const [value, setValue] = useState();

  const handleChange = (event: any) => {
    setValue(event.target.value);

    onChange({
      taskId: task.id,
      taskType: TaskType.SingleAnswer,
      isDone: true,
      variantsIds: [Number(event.target.value)]
    });
  }

  return (
    <>
      {task.variants?.map((variant) => {
        return (
          <div key={variant.id} className={classes.variant}>
            <Radio
              name={task.id}
              id={variant.id}
              value={variant.id}
              onChange={handleChange}
              checked={value == variant.id ? "t" : ""}
            ></Radio>
            <label htmlFor={variant.id.toString()} className={classes.variantLabel}>
              {variant.description}
            </label>
          </div>
        );
      })}
    </>
  );
}

export default SingleAnswersTask;
