import { useState } from "react";

import classes from "./editTask.module.css";
import { Task, TaskType, Variant } from "../../../types/task";
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

  return (
    <div className={classes.taskGrid}>
      <label className={classes.label} htmlFor="title">
        Текст задания
      </label>
      <TextAreaForm
        id="title"
        value={task.title}
        onChange={(event) => handleChange("title", event.target.value)}
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
        task={task}
        onChange={handleChange}
      />

      <label className={classes.label} htmlFor="maxPoints">
        Количество баллов
      </label>
      <InputForm
        type="number"
        id="maxPoints"
        value={task.maxPoints}
        onChange={(event) =>
          handleChange("maxPoints", Number.parseInt(event.target.value))
        }
      />
    </div>
  );
}

export default EditTask;
