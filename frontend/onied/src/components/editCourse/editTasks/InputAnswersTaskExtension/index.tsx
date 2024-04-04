import classes from "./index.module.css";
import { InputAnswersTask } from "../../../../types/task";
import InputForm from "../../../general/inputform/inputform";
import Button from "../../../general/button/button";
import TrashButton from "../../../general/trashButton";
import Checkbox from "../../../general/checkbox/checkbox";
import { useState } from "react";

function InputAnswersTaskExtension({
  id,
  task,
  onChange,
}: {
  id: string;
  task: InputAnswersTask;
  onChange: (attr: string, value: any) => void;
}) {
  const [accuracyStatus, setAccuracyStatus] = useState<boolean>(false);
  const [accuracyLevel, setAccuracyLevel] = useState<number>(0);

  const updateAnswer = (id: number, description: string) => {
    const newAnswers = task.answers;
    newAnswers!.find((v) => v.id == id)!.description = description;
    onChange("answers", newAnswers);
  };

  const addAnswer = (event: any) => {
    event.preventDefault();
    const newId =
      task.answers.length == 0
        ? 1
        : task.answers[task.answers.length - 1].id + 1;
    onChange("answers", task.answers!.concat({ id: newId, description: "" }));
  };

  const removeAnswer = (answerId: number) => {
    onChange(
      "answers",
      task.answers!.filter((v) => v.id != answerId)
    );
  };

  const changeAccuracyLevel = (event: any) => {
    const min = 0;
    const max = 1000;
    const value = Math.max(
      min,
      Math.min(max, Number.parseInt(event.target.value))
    );

    setAccuracyLevel(value);
    if (accuracyStatus) onChange("checkAccuracy", value);
  };

  const changeAccuracy = (event: any) => {
    setAccuracyStatus(event.target.checked);
    if (event.target.checked) onChange("checkAccuracy", accuracyLevel);
    else onChange("checkAccuracy", null);
  };

  return (
    <div id={id} className={classes.answersContainer}>
      {task.answers.map((answer) => {
        return (
          <div key={answer.id} className={classes.answer}>
            <InputForm
              style={{ width: "100%" }}
              value={answer.description}
              onChange={(event: any) =>
                updateAnswer(answer.id, event.target.value)
              }
            />
            {task.answers.length > 1 && (
              <TrashButton
                onClick={(event: any) => {
                  event.preventDefault();
                  removeAnswer(answer.id);
                }}
              />
            )}
          </div>
        );
      })}
      <Button
        onClick={(event: any) => addAnswer(event)}
        style={{ width: "fit-content" }}
      >
        добавить ответ
      </Button>
      <div className={classes.line}>
        <Checkbox onChange={changeAccuracy} />
        <p>Проверять на равенство с точностью до</p>
        <InputForm
          type="number"
          style={{ width: "5rem" }}
          value={accuracyLevel}
          onChange={changeAccuracyLevel}
        />
        <p>знаков после запятой</p>
      </div>
      <div className={classes.line}>
        <Checkbox
          onChange={(event: any) =>
            onChange("checkRegister", event.target.checked)
          }
        />
        <p>Игнорировать регистр</p>
      </div>
    </div>
  );
}

export default InputAnswersTaskExtension;
