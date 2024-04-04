import classes from "./editTask.module.css";
import { SingleAnswerTask, Task, TaskType } from "../../../types/task";
import InputForm from "../../general/inputform/inputform";
import TextAreaForm from "../../general/textareaform/textareaform";
import SelectForm from "../../general/selectform/select";
import SingleAnswerTaskExtension from "./SingleAnswerTaskExtension";

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

  const handleChange = (attr: string, value: any): void => {
    task = {
      ...task,
      [attr]: value,
    };
    onChange(task.id, task);
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
          "min-height": "4.5rem",
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
        onChange={(event) => handleChange("taskType", event.target.value)}
      />

      <label className={classes.label} htmlFor="variants">
        Ответ
      </label>
      <SingleAnswerTaskExtension
        id="variants"
        task={task as SingleAnswerTask}
        onChange={handleChange}
      />

      <label className={classes.label} htmlFor="maxPoints">
        Количество баллов
      </label>
      <InputForm
        type="number"
        id="maxPoints"
        min="0"
        maX="1000"
        style={{ width: "max-content" }}
        value={task.maxPoints}
        onChange={setMaxPoints}
      />
    </div>
  );
}

export default EditTask;
