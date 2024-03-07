import MultipleAnswersTask from "./multipleAnswersTask";
import SingleAnswersTask from "./singleAnswerTask";
import InputAnswer from "./inputAnswerTask";
import TaskTitle from "./taskTitle";
import taskType from "./taskType";
import ManualReviewTask from "./manualReviewTask";
import classes from "./tasks.module.css";

function GeneralTask({ task, index }) {
  const bodies = [
    SingleAnswersTask,
    MultipleAnswersTask,
    InputAnswer,
    ManualReviewTask,
  ];
  return (
    <div>
      <TaskTitle taskTitle={task.title} pointInfo={task.pointInfo}></TaskTitle>
      <div className={classes.taskBody}>{bodies[task.type]({ task })}</div>
    </div>
  );
}

export default GeneralTask;
