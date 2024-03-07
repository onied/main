import Checkbox from "../../general/checkbox/checkbox";
import classes from "./tasks.module.css";

function MultipleAnswersTask({ task }) {
  return (
    <>
      {task.variants.map((variant, index) => {
        return (
          <div key={task.id + "#" + index} className={classes.variant}>
            <Checkbox name={task.id} id={task.id + "#" + index}></Checkbox>
            <label
              htmlFor={task.id + "#" + index}
              className={classes.variantLabel}
            >
              {variant}
            </label>
          </div>
        );
      })}
    </>
  );
}

export default MultipleAnswersTask;
