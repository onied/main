import classes from "./index.module.css";
import {
  InputAnswersTask,
  MultipleAnswersTask,
  SingleAnswerTask,
  Task,
  TaskType,
} from "../../../../types/task";
import InputForm from "../../../general/inputform/inputform";
import SingleAnswerTaskExtension from "../SingleAnswerTaskExtension";
import MultipleAnswersTaskExtension from "../MultipleAnswersTaskExtension";
import InputAnswersTaskExtension from "../InputAnswersTaskExtension";
import Select from "../../../general/inputform/select";
import InputFormArea from "../../../general/inputform/inputFormArea";

function EditTask({
  task,
  onChange,
}: {
  task: Task;
  onChange: (task: Task) => void;
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
    [TaskType.InputAnswer]: () => (
      <InputAnswersTaskExtension
        id="answer"
        task={task as InputAnswersTask}
        onChange={handleChange}
      />
    ),
    [TaskType.ManualReview]: null,
  };

  const taskTypeConverters = {
    [TaskType.SingleAnswer]: (previousTask: Task) =>
      ({
        id: previousTask.id,
        title: previousTask.title,
        maxPoints: previousTask.maxPoints,
        taskType: TaskType.SingleAnswer,
        isNew: true,
        variants: [{ id: 0, description: "" }],
        rightVariant: 0,
      }) as SingleAnswerTask,
    [TaskType.MultipleAnswers]: (previousTask: Task) =>
      ({
        id: previousTask.id,
        title: previousTask.title,
        maxPoints: previousTask.maxPoints,
        taskType: TaskType.MultipleAnswers,
        isNew: true,
        variants: [
          { id: 0, description: "" },
          { id: 1, description: "" },
        ],
        rightVariants: [],
      }) as MultipleAnswersTask,
    [TaskType.InputAnswer]: (previousTask: Task) =>
      ({
        id: previousTask.id,
        title: previousTask.title,
        maxPoints: previousTask.maxPoints,
        taskType: TaskType.InputAnswer,
        isNew: true,
        answers: [
          { id: 0, description: "" },
          { id: 1, description: "" },
        ],
        isNumber: false,
        checkAccuracy: null,
        checkRegister: false,
      }) as InputAnswersTask,
    [TaskType.ManualReview]: (previousTask: Task) =>
      ({
        id: previousTask.id,
        title: previousTask.title,
        maxPoints: previousTask.maxPoints,
        taskType: TaskType.ManualReview,
        isNew: true,
      }) as Task,
  };

  const handleChange = (attr: string, value: any): void => {
    task = {
      ...task,
      [attr]: value,
    };
    onChange(task);
  };

  const setTaskType = (event: any) => {
    const taskType: TaskType = Number.parseInt(event.target.value);
    onChange(taskTypeConverters[taskType](task) as Task);
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
      <InputFormArea
        id="title"
        style={{
          minHeight: "8rem",
          resize: "vertical",
        }}
        maxLength="280"
        value={task.title}
        onChange={(event: any) => handleChange("title", event.target.value)}
      />

      <label className={classes.label} htmlFor="taskType">
        Тип задания
      </label>
      <Select id="taskType" value={task.taskType} onChange={setTaskType}>
        {options.map((opt) => (
          <option key={opt.value} value={opt.value}>
            {opt.label}
          </option>
        ))}
      </Select>

      {task.taskType === null
        ? null
        : answerExtensions[task.taskType] && (
            <>
              <label className={classes.label} htmlFor="answer">
                Ответ
              </label>
              {answerExtensions[task.taskType]!()}
            </>
          )}

      <label className={classes.label} htmlFor="maxPoints">
        Количество баллов
      </label>
      <InputForm
        type="number"
        id="maxPoints"
        style={{ width: "5rem" }}
        value={task.maxPoints}
        onChange={setMaxPoints}
      />
    </div>
  );
}

export default EditTask;
