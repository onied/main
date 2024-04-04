import classes from "./editTask.module.css";
import {
  MultipleAnswersTask,
  SingleAnswerTask,
  Task,
  TaskType,
} from "../../../types/task";
import InputForm from "../../general/inputform/inputform";
import TextAreaForm from "../../general/textareaform/textareaform";
import SelectForm from "../../general/selectform/select";
import SingleAnswerTaskExtension from "./SingleAnswerTaskExtension";
import MultipleAnswersTaskExtension from "./MultipleAnswersTaskExtension";
import SingleAnswersTask from "../../blocks/tasks/singleAnswerTask";

function EditTask({
  task,
  onChange,
}: {
  task: Task;
  onChange: (id: number, task: Task) => void;
}) {
  const options = [
    { value: TaskType.SingleAnswer, label: "Задание с выбором одного ответа" },
    {
      value: TaskType.MultipleAnswers,
      label: "Задание с выбором многих ответов",
    },
    { value: TaskType.ManualReview, label: "Задание с ручной проверкой" },
    { value: TaskType.InputAnswer, label: "Задание с текстовым ответом" },
  ];

  const answerExtensions = {
    [TaskType.SingleAnswer]: () => (
      <SingleAnswerTaskExtension
        id="answer"
        task={task as SingleAnswerTask}
        onChange={handleChange}
      />
    ),
    [TaskType.MultipleAnswers]: () => (
      <MultipleAnswersTaskExtension
        id="answer"
        task={task as MultipleAnswersTask}
        onChange={handleChange}
      />
    ),
    [TaskType.InputAnswer]: null,
    [TaskType.ManualReview]: null,
  };

  const taskTypeConverters = {
    [TaskType.SingleAnswer]: (previousTask: Task) =>
      ({
        id: previousTask.id,
        title: previousTask.title,
        maxPoints: previousTask.maxPoints,
        taskType: TaskType.SingleAnswer,
        variants: [{ id: 0, description: "" }],
        rightVariant: 0,
      }) as SingleAnswerTask,
    [TaskType.MultipleAnswers]: (previousTask: Task) =>
      ({
        id: previousTask.id,
        title: previousTask.title,
        maxPoints: previousTask.maxPoints,
        taskType: TaskType.MultipleAnswers,
        variants: [{ id: 0, description: "" }],
        rightVariants: [],
      }) as MultipleAnswersTask,
    [TaskType.InputAnswer]: (task: Task) => null,
    [TaskType.ManualReview]: (task: Task) => null,
  };

  const handleChange = (attr: string, value: any): void => {
    task = {
      ...task,
      [attr]: value,
    };
    onChange(task.id, task);
  };

  const setTaskType = (event: any) => {
    const taskType: TaskType = Number.parseInt(event.target.value);
    onChange(task.id, taskTypeConverters[taskType](task) as Task);
  };

  const setMaxPoints = (event: any) => {
    const min = 0;
    const max = 1000;
    var inputValue = Number.parseInt(event.target.value);
    handleChange("maxPoints", Math.max(min, Math.min(max, inputValue)));
  };

  return (
    <div className={classes.taskContainer}>
      <label className={classes.label} htmlFor="title">
        Текст задания
      </label>
      <TextAreaForm
        id="title"
        style={{
          minHeight: "4.5rem",
          resize: "vertical",
        }}
        maxLength="280"
        value={task.title}
        onChange={(event: any) => handleChange("title", event.target.value)}
      />

      <label className={classes.label} htmlFor="taskType">
        Тип задания
      </label>
      <SelectForm
        options={options}
        id="taskType"
        value={task.taskType}
        onChange={setTaskType}
      />

      <label className={classes.label} htmlFor="answer">
        Ответ
      </label>
      {task.taskType !== null
        ? answerExtensions[task.taskType]
          ? answerExtensions[task.taskType]!()
          : null
        : null}

      <label className={classes.label} htmlFor="maxPoints">
        Количество баллов
      </label>
      <InputForm
        type="number"
        id="maxPoints"
        style={{ width: "max-content" }}
        value={task.maxPoints}
        onChange={setMaxPoints}
      />
    </div>
  );
}

export default EditTask;
