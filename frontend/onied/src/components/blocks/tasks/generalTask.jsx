import MultipleAnswersTask from "./multipleAnswersTask";
import SingleAnswersTask from "./singleAnswerTask";
import InputAnswer from "./inputAnswerTask";
import TaskTitle from "./taskTitle";
import ManualReviewTask from "./manualReviewTask";
import classes from "./tasks.module.css";

function GeneralTask({ task, index, taskPoints, onChange }) {
  const bodies = [
    SingleAnswersTask,
    MultipleAnswersTask,
    InputAnswer,
    ManualReviewTask,
  ];

  const handleChange = (input) => onChange(index, input);

  return (
    <div>
      <TaskTitle
        taskTitle={task.title}
        pointInfo={{
          checked: taskPoints != null && taskPoints.points != null,
          points:
            taskPoints != null && taskPoints.points != null
              ? taskPoints.points
              : 0,
          maxPoints: task.maxPoints,
        }}
      ></TaskTitle>
      <div className={classes.taskBody}>
        {bodies[task.taskType]({
          task: task,
          onChange: handleChange,
          initialValue: taskPoints?.content,
        })}
      </div>
    </div>
  );
}

export default GeneralTask;
