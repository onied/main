import Accordion from "@mui/material/Accordion";
import AccordionDetails from "@mui/material/AccordionDetails";
import AccordionSummary from "@mui/material/AccordionSummary";
import Arrow from "../../../assets/arrow.svg";
import "./muiAccordionOverride.css";
import classes from "./taskChecking.module.css";
import { useEffect, useState } from "react";
import { Link, Navigate } from "react-router-dom";
import { StyledEngineProvider } from "@mui/material/styles";
import TaskToCheckDescription from "./taskToCheckDescription";
import BeatLoader from "react-spinners/BeatLoader";
import NoAccess from "../../general/responses/noAccess/noAccess";
import NoContent from "../../general/responses/noContent/noContent";

function TasksToCheck() {
  const [loadStatus, setLoadStatus] = useState(0);
  const [coursesWithTasksList, setTaskList] = useState<
    Array<CourseWithTasks> | undefined
  >();

  useEffect(() => {
    setTimeout(() => {
      setLoadStatus(200);
      setTaskList([]);
      // setTaskList([
      //   {
      //     courseId: 1,
      //     courseName: "Название курса.",
      //     tasksToCheck: [
      //       {
      //         taskId: "1",
      //         moduleName: "Первый модуль",
      //         blockName: "Первый блок",
      //         taskTitle: "Напишите эссе",
      //       },
      //       {
      //         taskId: "2",
      //         moduleName: "Первый модуль",
      //         blockName: "Первый блок",
      //         taskTitle: "Напишите эссе",
      //       },
      //       {
      //         taskId: "3",
      //         moduleName: "Первый модуль",
      //         blockName: "Первый блок",
      //         taskTitle: "Напишите эссе",
      //       },
      //       {
      //         taskId: "4",
      //         moduleName: "Первый модуль",
      //         blockName: "Первый блок",
      //         taskTitle: "Напишите эссе",
      //       },
      //     ],
      //   },
      //   {
      //     courseId: 2,
      //     courseName: "Название курса. Второй",
      //     tasksToCheck: [
      //       {
      //         taskId: "5",
      //         moduleName: "Первый модуль",
      //         blockName: "Первый блок",
      //         taskTitle: "Напишите эссе",
      //       },
      //       {
      //         taskId: "6",
      //         moduleName: "Первый модуль",
      //         blockName: "Первый блок",
      //         taskTitle: "Напишите эссе",
      //       },
      //     ],
      //   },
      //   {
      //     courseId: 3,
      //     courseName: "Название курса. Третий",
      //     tasksToCheck: [
      //       {
      //         taskId: "7",
      //         moduleName: "Первый модуль",
      //         blockName: "Первый блок",
      //         taskTitle: "Напишите эссе",
      //       },
      //       {
      //         taskId: "8",
      //         moduleName: "Первый модуль",
      //         blockName: "Первый блок",
      //         taskTitle: "Напишите эссе",
      //       },
      //     ],
      //   },
      // ]);
    }, 750);
  }, []);

  switch (loadStatus) {
    case 0:
      return (
        <BeatLoader
          cssOverride={{ margin: "30px 30px" }}
          color="var(--accent-color)"
        ></BeatLoader>
      );
    case 401:
      return <Navigate to="/login"></Navigate>;
    case 403:
      return <NoAccess>У вас нет прав для доступа к этой странице</NoAccess>;
  }

  if (loadStatus !== 200 || coursesWithTasksList === undefined) return <></>;

  if (coursesWithTasksList.length == 0)
    return (
      <NoContent>Для вас нет ответов на задания, требующих проверки</NoContent>
    );

  return (
    <StyledEngineProvider injectFirst>
      <div className={classes.accordionsWrapper}>
        {coursesWithTasksList.map((courseWithTask) => (
          <Accordion
            key={courseWithTask.courseId}
            className={classes.accordion}
          >
            <AccordionSummary
              className={classes.accordionSummary}
              expandIcon={<img src={Arrow} />}
            >
              <p>{courseWithTask.courseName}</p>
              <div className={classes.uncheckedTasksCount}>
                <span>{courseWithTask.tasksToCheck.length}</span>
              </div>
            </AccordionSummary>
            {courseWithTask.tasksToCheck.map((task) => (
              <AccordionDetails
                key={task.taskId}
                className={classes.accordionDetails}
              >
                <TaskToCheckDescription {...task}></TaskToCheckDescription>
                <Link
                  to={task.taskId}
                  className={classes.checkButton}
                >
                  проверить
                </Link>
              </AccordionDetails>
            ))}
          </Accordion>
        ))}
      </div>
    </StyledEngineProvider>
  );
}

type CourseWithTasks = {
  courseId: number;
  courseName: string;
  tasksToCheck: Array<TaskToCheckInfo>;
};

export type TaskToCheckInfo = {
  taskId: string;
  moduleName: string;
  blockName: string;
  taskTitle: string;
};

export default TasksToCheck;
