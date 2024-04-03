import { useState } from "react";
import InputForm from "../../../general/inputform/inputform";
import classes from "./editSingleAnswerTask.module.css";
import { Task, TaskType } from "../../../../types/task";
import TextAreaForm from "../../../general/textareaform/textareaform";
import SelectForm from "../../../general/selectform/select";

function EditSingleAnswerTask({ task }: { task: Task }) {
  const [title, setTitle] = useState<string>(task.title);
  const [taskType, setTaskType] = useState<TaskType>(task.taskType);
  const [maxPoints, setMaxPoints] = useState<number>(task.maxPoints);

  const options = [
    { value: TaskType.ManualReview, label: "Задание с ручной проверкой" },
    { value: TaskType.InputAnswer, label: "Задание с текстовым ответом" },
    { value: TaskType.SingleAnswer, label: "Задание с выбором одного ответа" },
    {
      value: TaskType.MultipleAnswers,
      label: "Задание с выбором многих ответов",
    },
  ];

  return (
    <div className={classes.taskGrid}>
      <label className={classes.label} htmlFor="title">
        Текст задания
      </label>
      <TextAreaForm id="title" onChange={setTitle} />
      <label className={classes.label} htmlFor="title2">
        Текст задания
      </label>
      <InputForm id="title2" onChange={setTitle} />
      <label className={classes.label} htmlFor="task_type">
        Тип задания
      </label>
      <SelectForm
        options={options}
        id="task_type"
        onChange={(opt: any) => console.log(opt)}
      />
    </div>
  );
}

export default EditSingleAnswerTask;
