import Accordion from "@mui/material/Accordion";
import AccordionDetails from "@mui/material/AccordionDetails";
import AccordionSummary from "@mui/material/AccordionSummary";
import Arrow from "../../../assets/arrow.svg";
import "./muiAccordionOverride.css";
import classes from "./taskChecking.module.css";
import { useState } from "react";
import { Link } from "react-router-dom";

function TaskChecking() {
  const [coursesWithTasksList, setTaskList] = useState<Array<CourseWithTasks>>([
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
      courseName: "Название курса. 2",
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
      courseName: "Название курса. 3",
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

  return (
    <div className={classes.accordionsWrapper}>
      {coursesWithTasksList.map((courseWithTask) => (
        <Accordion className={classes.accordion}>
          <AccordionSummary
            className={classes.accordionSummary}
            expandIcon={<img src={Arrow} />}
          >
            <p>{courseWithTask.courseName}</p>
          </AccordionSummary>
          {courseWithTask.tasksToCheck.map((task) => (
            <AccordionDetails className={classes.accordionDetails}>
              <TaskDescription {...task}></TaskDescription>
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
  );
}

function TaskDescription(props: TaskToCheck) {
  return (
    <div className={classes.taskWrapper}>
      <div className={classes.taskName}>{props.taskTitle}</div>
      <div className={classes.moduleAndBlockName}>
        <span>{props.moduleName}</span>
        <img src={Arrow} />
        <span>{props.blockName}</span>
      </div>
    </div>
  );
}

type CourseWithTasks = {
  courseId: number;
  courseName: string;
  tasksToCheck: Array<TaskToCheck>;
};

type TaskToCheck = {
  taskId: number;
  moduleName: string;
  blockName: string;
  taskTitle: string;
};

export default TaskChecking;
