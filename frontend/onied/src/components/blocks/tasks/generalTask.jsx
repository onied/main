import MultipleAnswersTask from "./multipleAnswersTask";
import SingleAnswersTask from "./singleAnswerTask";
import InputAnswer from "./inputAnswerTask";
import TaskTitle from "./taskTitle";
import taskType from "./taskType";
import ManualReviewTask from "./manualReviewTask";
import classes from "./tasks.module.css";

function GeneralTask({ task, index }) {
  return (
    <div>
      <TaskTitle taskTitle={task.title} pointInfo={task.pointInfo}></TaskTitle>
      <div className={classes.taskBody}>
        {task.type == taskType.SINGLE_ANSWER ? (
          <SingleAnswersTask task={task}></SingleAnswersTask>
        ) : task.type == taskType.MULTIPLE_ANSWERS ? (
          <MultipleAnswersTask task={task}></MultipleAnswersTask>
        ) : task.type == taskType.INPUT_ANSWER ? (
          <InputAnswer task={task}></InputAnswer>
        ) : task.type == taskType.REVIEW_ANSWER ? (
          <ManualReviewTask task={task}></ManualReviewTask>
        ) : (
          <></>
        )}
      </div>
    </div>
  );
}

export default GeneralTask;
