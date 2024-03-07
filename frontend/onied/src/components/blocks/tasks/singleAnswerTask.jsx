import Radio from "../../general/radio/radio";
import classes from "./tasks.module.css";

function SingleAnswersTask({ task }) {
  return (
    <>
      {task.variants.map((variant, index) => {
        return (
          <div key={task.id + "#" + index} className={classes.variant}>
            <Radio name={task.id} id={task.id + "#" + index}></Radio>
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

export default SingleAnswersTask;
