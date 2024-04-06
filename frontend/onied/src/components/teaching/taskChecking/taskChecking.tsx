import Accordion from "@mui/material/Accordion";
import AccordionDetails from "@mui/material/AccordionDetails";
import AccordionSummary from "@mui/material/AccordionSummary";
import Arrow from "../../../assets/arrow.svg";
import classes from "./taskChecking.module.css";
import { useState } from "react";

function TaskChecking() {
  const [coursesWithTasksList, setTaskList] = useState<Array<CourseWithTasks>>([
    {
      courseId: 1,
      courseName: "Название курса.",
      tasksToCheck: [
        {
          moduleName: "Первый модуль",
          blockName: "Первый блок",
          taskTitle: "Напишите эссе",
          isChecked: false,
        },
        {
          moduleName: "Первый модуль",
          blockName: "Первый блок",
          taskTitle: "Напишите эссе",
          isChecked: false,
        },
      ],
    },
    {
      courseId: 2,
      courseName: "Название курса. 2",
      tasksToCheck: [
        {
          moduleName: "Первый модуль",
          blockName: "Первый блок",
          taskTitle: "Напишите эссе",
          isChecked: false,
        },
        {
          moduleName: "Первый модуль",
          blockName: "Первый блок",
          taskTitle: "Напишите эссе",
          isChecked: false,
        },
      ],
    },
  ]);

  return (
    <div>
      {coursesWithTasksList.map((courseWithTask) => (
        <Accordion>
          <AccordionSummary>{courseWithTask.courseName}</AccordionSummary>
          {courseWithTask.tasksToCheck.map((task) => (
            <AccordionDetails>
              <TaskDescription {...task}></TaskDescription>
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
      <div>{props.taskTitle}</div>
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
  moduleName: string;
  blockName: string;
  taskTitle: string;
  isChecked: boolean;
};

export default TaskChecking;
