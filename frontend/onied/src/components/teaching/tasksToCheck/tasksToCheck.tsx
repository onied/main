import Accordion from "@mui/material/Accordion";
import AccordionDetails from "@mui/material/AccordionDetails";
import AccordionSummary from "@mui/material/AccordionSummary";
import Arrow from "../../../assets/arrow.svg";
import classes from "./taskChecking.module.css";
import { useEffect, useState } from "react";
import { Link, Navigate } from "react-router-dom";
import { StyledEngineProvider } from "@mui/material/styles";
import TaskToCheckDescription from "./taskToCheckDescription";
import NoAccess from "../../general/responses/noAccess/noAccess";
import NoContent from "../../general/responses/noContent/noContent";
import api from "../../../config/axios";
import CustomBeatLoader from "../../general/customBeatLoader";

function TasksToCheck() {
  const [loadStatus, setLoadStatus] = useState(0);
  const [coursesWithTasksList, setCoursesWithTasksList] = useState<
    Array<CourseWithTasks> | undefined
  >();

  useEffect(() => {
    api
      .get("teaching/tasks-to-check-list")
      .then((response) => {
        setCoursesWithTasksList(response.data);
        setLoadStatus(200);
      })
      .catch((error) => {
        console.log(error);
      });
  }, []);

  switch (loadStatus) {
    case 0:
      return <CustomBeatLoader />;
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
              <p>{courseWithTask.title}</p>
              <div className={classes.uncheckedTasksCount}>
                <span>{courseWithTask.tasksToCheck.length}</span>
              </div>
            </AccordionSummary>
            <div className={classes.accordionDetailsWrapper}>
              {courseWithTask.tasksToCheck.map((task) => (
                <AccordionDetails
                  key={task.index}
                  className={classes.accordionDetails}
                >
                  <TaskToCheckDescription {...task}></TaskToCheckDescription>
                  <Link to={task.index} className={classes.checkButton}>
                    проверить
                  </Link>
                </AccordionDetails>
              ))}
            </div>
          </Accordion>
        ))}
      </div>
    </StyledEngineProvider>
  );
}

type CourseWithTasks = {
  courseId: number;
  title: string;
  tasksToCheck: Array<TaskToCheckInfo>;
};

export type TaskToCheckInfo = {
  index: string;
  moduleTitle: string;
  blockTitle: string;
  title: string;
};

export default TasksToCheck;
