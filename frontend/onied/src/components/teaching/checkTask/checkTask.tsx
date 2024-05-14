import Avatar from "react-avatar";
import ButtonGoBack from "../../../components/general/buttonGoBack/buttonGoBack";
import classes from "./checkTask.module.css";
import Markdown from "react-markdown";
import Button from "../../../components/general/button/button";
import { useEffect, useState } from "react";
import api from "../../../config/axios";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import NotFound from "../../general/responses/notFound/notFound";
import NoAccess from "../../general/responses/noAccess/noAccess";
import CustomBeatLoader from "../../general/customBeatLoader";

type CourseInfo = {
  title: string;
};

type ModuleInfo = {
  course: CourseInfo;
  index: number;
  title: string;
};

type BlockInfo = {
  module: ModuleInfo;
  index: number;
  title: string;
};

type TaskInfo = {
  block: BlockInfo;
  title: string;
  maxPoints: number;
};

type TaskCheckInfo = {
  task: TaskInfo;
  content: string;
  checked: boolean;
  points: number;
};

type Errors = {
  points: string | undefined;
};

function CheckTaskComponent() {
  const navigate = useNavigate();
  const { taskCheckId } = useParams(); // taskCheckId: guid, no dependency on module or block or task
  const [loadStatus, setLoadStatus] = useState(0); // 0 - still loading, other -> statusCode
  const [taskInfo, setTaskInfoRaw] = useState<TaskCheckInfo | undefined>();
  const [errors, setErrors] = useState<Errors>({ points: undefined });
  const [points, setPointsRaw] = useState(0);
  const notFound = <NotFound>Задание на проверке не было найдено.</NotFound>;
  const noAccess = (
    <NoAccess>У вас нет права на проверку этого задания.</NoAccess>
  );
  const setPoints = (points: number) => {
    if (points < 0) setPointsRaw(0);
    else if (points > taskInfo!.task.maxPoints)
      setPointsRaw(taskInfo!.task.maxPoints);
    else setPointsRaw(points);
    setErrors({
      points: undefined,
    });
  };
  const setTaskInfo = (taskInfo: TaskCheckInfo | undefined) => {
    setTaskInfoRaw(taskInfo);
    if (taskInfo !== undefined) setPointsRaw(taskInfo.points);
    else setPointsRaw(0);
  };
  const saveAndReturn = () => {
    setErrors({
      points: undefined,
    });
    api
      .put("/teaching/check/" + encodeURIComponent(taskCheckId!), {
        points: points,
      })
      .then((_) => {
        navigate(-1); // go back to task check list
      })
      .catch((error) => {
        if (!error.response) return;
        if (error.response.status !== 400)
          return setLoadStatus(error.response.status);
        if (error.response.data.errors.Points)
          setErrors({ points: "Значение некорректно." });
      });
  };

  useEffect(() => {
    setLoadStatus(0);
    setTaskInfo(undefined);
    api
      .get("/teaching/check/" + encodeURIComponent(taskCheckId!))
      .then((response) => {
        setTaskInfo(response.data);
        setLoadStatus(200);
      })
      .catch((error) => {
        setLoadStatus(error.response?.status);
      });
  }, [taskCheckId]);

  if (loadStatus === 0) return <CustomBeatLoader />;
  if (loadStatus === 401) return <Navigate to="/login"></Navigate>;
  if (loadStatus === 404) return notFound;
  if (loadStatus === 403) return noAccess;
  if (loadStatus !== 200 || taskInfo === undefined) return <></>;

  return (
    <div className={classes.container}>
      <div className={classes.pageHeader}>
        <ButtonGoBack onClick={() => navigate(-1)}>
          ⟵ вернуться без сохранения
        </ButtonGoBack>
        <div className={classes.header}>
          <h1 className={classes.courseTitle}>
            {taskInfo.task.block.module.course.title}
          </h1>
          <div className={classes.headline}>
            <div className={classes.moduleBlockContainer}>
              <div className={classes.module}>
                <label htmlFor="moduleTitle" className={classes.label}>
                  Модуль {taskInfo.task.block.module.index + 1}:
                </label>
                <p id="moduleTitle" className={classes.moduleTitle}>
                  {taskInfo.task.block.module.title}
                </p>
              </div>
              <div className={classes.block}>
                <label htmlFor="blockTitle" className={classes.label}>
                  Блок {taskInfo.task.block.index + 1}:
                </label>
                <p id="blockTitle" className={classes.blockTitle}>
                  {taskInfo.task.block.title}
                </p>
              </div>
            </div>
            <div className={classes.task}>
              <label htmlFor="taskTitle" className={classes.taskLabel}>
                Задание:
              </label>
              <p className={classes.taskTitle} id="taskTitle">
                {taskInfo.task.title}
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
          <Markdown>{taskInfo.content}</Markdown>
        </div>
      </div>
      <div className={classes.pointsContainerContainer}>
        <div className={classes.pointsContainer}>
          <label htmlFor="points" className={classes.label}>
            Количество баллов:
          </label>
          <div className={classes.badge + " " + classes.points} id="points">
            <div className={classes.spinbox}>
              <input
                type="number"
                className={classes.spinboxContent}
                max={taskInfo.task.maxPoints}
                min={0}
                value={points}
                style={{
                  width: taskInfo.task.maxPoints.toString().length + "rem",
                }}
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
            /<p className={classes.maxPoints}>{taskInfo.task.maxPoints}</p>
          </div>
        </div>
        {errors.points ? (
          <p className={classes.error}>{errors.points}</p>
        ) : (
          <></>
        )}
      </div>
      <div className={classes.footer}>
        <div className={classes.line}></div>
        <div className={classes.saveChanges}>
          <Button onClick={saveAndReturn}>
            сохранить и вернуться в список проверки
          </Button>
        </div>
      </div>
    </div>
  );
}

export default CheckTaskComponent;
