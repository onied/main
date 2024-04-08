import Accordion from "@mui/material/Accordion";
import AccordionDetails from "@mui/material/AccordionDetails";
import AccordionSummary from "@mui/material/AccordionSummary";
import Arrow from "../../../assets/arrow.svg";
import "./muiAccordionOverride.css";
import classes from "./taskChecking.module.css";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { StyledEngineProvider } from "@mui/material/styles";
import TaskToCheckDescription from "./taskToCheckDescription";
import BeatLoader from "react-spinners/BeatLoader";

function TaskChecking() {
  const [coursesWithTasksList, setTaskList] = useState<
    Array<CourseWithTasks> | undefined
  >();

  useEffect(() => {
    setTimeout(() => {
      setTaskList([
        {
          courseId: 1,
          courseName: "Название курса.",
          tasksToCheck: [
            {
              taskId: 1,
              moduleName: "Первый модуль",
              blockName: "Первый блок",
              taskTitle: "Напишите эссе",
            },
            {
              taskId: 2,
              moduleName: "Первый модуль",
              blockName: "Первый блок",
              taskTitle: "Напишите эссе",
            },
            {
              taskId: 3,
              moduleName: "Первый модуль",
              blockName: "Первый блок",
              taskTitle: "Напишите эссе",
            },
            {
              taskId: 4,
              moduleName: "Первый модуль",
              blockName: "Первый блок",
              taskTitle: "Напишите эссе",
            },
          ],
        },
        {
          courseId: 2,
          courseName: "Название курса. Второй",
          tasksToCheck: [
            {
              taskId: 5,
              moduleName: "Первый модуль",
              blockName: "Первый блок",
              taskTitle: "Напишите эссе",
            },
            {
              taskId: 6,
              moduleName: "Первый модуль",
              blockName: "Первый блок",
              taskTitle: "Напишите эссе",
            },
          ],
        },
        {
          courseId: 3,
          courseName: "Название курса. Третий",
          tasksToCheck: [
            {
              taskId: 7,
              moduleName: "Первый модуль",
              blockName: "Первый блок",
              taskTitle: "Напишите эссе",
            },
            {
              taskId: 8,
              moduleName: "Первый модуль",
              blockName: "Первый блок",
              taskTitle: "Напишите эссе",
            },
          ],
        },
      ]);
    }, 750);
  }, []);

  if (coursesWithTasksList == undefined)
    return (
      <BeatLoader
        cssOverride={{ margin: "30px 30px" }}
        color="var(--accent-color)"
      ></BeatLoader>
    );

  return (
    <StyledEngineProvider injectFirst>
      <div className={classes.accordionsWrapper}>
        {coursesWithTasksList.map((courseWithTask) => (
          <Accordion className={classes.accordion}>
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
              <AccordionDetails className={classes.accordionDetails}>
                <TaskToCheckDescription {...task}></TaskToCheckDescription>
                <Link
                  to={"/check/" + task.taskId}
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
  tasksToCheck: Array<TaskToCheck>;
};

export type TaskToCheck = {
  taskId: number;
  moduleName: string;
  blockName: string;
  taskTitle: string;
};

export default TaskChecking;
