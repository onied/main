import classes from "./index.module.css";
import { InputAnswersTask } from "../../../../types/task";
import InputForm from "../../../general/inputform/inputform";
import Button from "../../../general/button/button";
import TrashButton from "../../../general/trashButton";
import Checkbox from "../../../general/checkbox/checkbox";
import { useState } from "react";
import SwitchForm from "../../../general/switch";

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
    newAnswers!.find((v) => v.id == id)!.answer = description;
    onChange("answers", newAnswers);
  };

  const addAnswer = (event: any) => {
    event.preventDefault();
    const newId =
      task.answers.length == 0
        ? 1
        : task.answers[task.answers.length - 1].id + 1;
    onChange(
      "answers",
      task.answers!.concat({ id: newId, answer: "", isNew: true })
    );
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
    if (accuracyStatus) onChange("accuracy", value);
  };

  const changeAccuracy = (event: any) => {
    setAccuracyStatus(event.target.checked);
    if (event.target.checked) onChange("accuracy", accuracyLevel);
    else onChange("accuracy", null);
  };

  return (
    <div id={id} className={classes.answersContainer}>
      {task.answers.map((answer) => {
        return (
          <div key={answer.id} className={classes.answer}>
            <InputForm
              style={{ width: "100%" }}
              value={answer.answer}
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
        <p>строка</p>
        <SwitchForm
          checked={task.isNumber}
          onChange={(event) => {
            onChange("isNumber", event.target.checked);
          }}
        />
        <p>число</p>
      </div>
      {task.isNumber ? (
        <div className={classes.line}>
          <Checkbox checked={task.accuracy != null} onChange={changeAccuracy} />
          <p>
            проверять на равенство с точностью до
            <InputForm
              type="number"
              style={{ width: "5rem", margin: "0 0.5rem" }}
              value={accuracyLevel}
              onChange={changeAccuracyLevel}
            />
            знаков после запятой
          </p>
        </div>
      ) : (
        <div className={classes.line}>
          <Checkbox
            checked={task.isCaseSensitive}
            onChange={(event: any) =>
              onChange("isCaseSensitive", event.target.checked)
            }
          />
          <p>чувствительность к регистру</p>
        </div>
      )}
    </div>
  );
}

export default InputAnswersTaskExtension;
