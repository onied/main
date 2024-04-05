import Avatar from "react-avatar";
import ButtonGoBack from "../../../components/general/buttonGoBack/buttonGoBack";
import classes from "./checkTask.module.css";
import Markdown from "react-markdown";
import Button from "../../../components/general/button/button";
import { useState } from "react";

function CheckTask() {
  const [taskInfo, setTaskInfo] = useState({
    courseTitle: "Название курса. Как я встретил вашу маму. Осуждаю.",
    moduleNumber: 1,
    moduleTitle: "Первый модуль",
    blockNumber: 2,
    blockTitle: "Второй блок",
    taskNumber: 4,
    taskTitle: "Напишите эссе на тему: “Как я провел лето”",
    maxPoints: 5,
    taskContents:
      "Здравствуйте учитель я нихрена не сделал поставьте 5/5 пжпж asdfadsf",
    checked: false,
    points: 0,
  });
  const [points, setPointsRaw] = useState(taskInfo.points);
  const setPoints = (points: number) => {
    if (points < 0) setPointsRaw(0);
    else if (points > taskInfo.maxPoints) setPointsRaw(taskInfo.maxPoints);
    else setPointsRaw(points);
  };

  return (
    <div className={classes.pageContainer}>
      <div className={classes.container}>
        <div className={classes.pageHeader}>
          <ButtonGoBack>⟵ вернуться без сохранения</ButtonGoBack>
          <div className={classes.header}>
            <h1 className={classes.courseTitle}>{taskInfo.courseTitle}</h1>
            <div className={classes.headline}>
              <div className={classes.moduleBlockContainer}>
                <div className={classes.module}>
                  <label htmlFor="moduleTitle" className={classes.label}>
                    Модуль {taskInfo.moduleNumber}:
                  </label>
                  <p id="moduleTitle" className={classes.moduleTitle}>
                    {taskInfo.moduleTitle}
                  </p>
                </div>
                <div className={classes.block}>
                  <label htmlFor="blockTitle" className={classes.label}>
                    Блок {taskInfo.blockNumber}:
                  </label>
                  <p id="blockTitle" className={classes.blockTitle}>
                    {taskInfo.blockTitle}
                  </p>
                </div>
              </div>
              <div className={classes.task}>
                <label htmlFor="taskTitle" className={classes.taskLabel}>
                  Задание {taskInfo.taskNumber}:
                </label>
                <p className={classes.taskTitle} id="taskTitle">
                  {taskInfo.taskTitle}
                </p>
              </div>
            </div>
            <div className={classes.studentBlock}>
              <div className={classes.student}>
                <Avatar round size="50" color="#c4c4c4" value={" "}></Avatar>
                <p className={classes.studentName}>Аноним</p>
              </div>
              <div
                className={classes.badge}
                style={{
                  backgroundColor: taskInfo.checked ? "#419341" : "#282828",
                }}
              >
                {taskInfo.checked ? "проверено" : "непроверено"}
              </div>
            </div>
          </div>
        </div>
        <div className={classes.contents}>
          <div className={classes.contentsText}>
            <Markdown>{taskInfo.taskContents}</Markdown>
          </div>
        </div>
        <div className={classes.pointsContainer}>
          <label htmlFor="points" className={classes.label}>
            Количество баллов:
          </label>
          <div className={classes.badge + " " + classes.points} id="points">
            <div className={classes.spinbox}>
              <input
                type="number"
                className={classes.spinboxContent}
                max={taskInfo.maxPoints}
                min={0}
                value={points}
                style={{ width: taskInfo.maxPoints.toString().length + "rem" }}
                onChange={(event) => {
                  setPoints(event.target.valueAsNumber);
                  event.target.value = event.target.valueAsNumber.toString();
                }}
              ></input>
              <div className={classes.spinButtons}>
                <button
                  className={classes.spinButton}
                  onClick={() => setPoints(points + 1)}
                >
                  <span className={classes.spinButtonArrowUp}>⌃</span>
                </button>
                <button
                  className={classes.spinButton}
                  onClick={() => setPoints(points - 1)}
                >
                  <span className={classes.spinButtonArrowDown}>⌄</span>
                </button>
              </div>
            </div>
            /<p className={classes.maxPoints}>{taskInfo.maxPoints}</p>
          </div>
        </div>
        <div className={classes.footer}>
          <div className={classes.line}></div>
          <div className={classes.saveChanges}>
            <Button onClick={() => console.log("todo: bind a function")}>
              сохранить и вернуться в список проверки
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default CheckTask;
