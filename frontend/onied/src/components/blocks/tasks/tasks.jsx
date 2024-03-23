import Button from "../../general/button/button";
import GeneralTask from "./generalTask";
import classes from "./tasks.module.css";
import BeatLoader from "react-spinners/BeatLoader";
import { useEffect, useState } from "react";
import api from "../../../config/axios";

function Tasks({ courseId, blockId }) {
  const [tasks, setTasks] = useState();
  const [found, setFound] = useState();
  const [taskInputs, setTaskInputs] = useState();
  const [taskPointsSequence, setTaskPointsSequence] = useState();

  const handleChange = (inputIndex, input) => {
    const newTaskInputs = [...taskInputs];
    newTaskInputs[inputIndex] = input;
    console.log(newTaskInputs);
    setTaskInputs(newTaskInputs);
  };

  useEffect(() => {
    setTaskPointsSequence(undefined);
    api
      .get(
        Config.CoursesBackend +
          "courses/" +
          courseId +
          "/tasks/" +
          blockId +
          "/points"
      )
      .then((response) => {
        console.log(response.data);
        setTaskPointsSequence(response.data);
      })
      .catch((error) => {
        console.log(error);
        setTaskPointsSequence(null);
      });
  }, [courseId, blockId]);

  useEffect(() => {
    setFound(undefined);
    axios
      .get(Config.CoursesBackend + "courses/" + courseId + "/tasks/" + blockId)
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setTasks(response.data);
        setTaskInputs(
          response.data.tasks.map((task) => {
            return {
              taskId: task.id,
              isDone: false,
              taskType: task.taskType,
            };
          })
        );
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, [courseId, blockId]);

  if (found == null || taskPointsSequence == null)
    return <BeatLoader color="var(--accent-color)"></BeatLoader>;
  if (!found) return <></>;

  return (
    <form className={classes.tasksContainer} action="post">
      <h2>{tasks.title}</h2>
      {tasks.tasks.map((task, index) => {
        return (
          <GeneralTask
            key={index}
            task={task}
            index={index}
            onChange={handleChange}
            taskPoints={taskPointsSequence[index]}
          />
        );
      })}
      <Button
        type="submit"
        onClick={(e) => {
          e.preventDefault();
          console.log(taskInputs);

          setTaskPointsSequence(undefined);
          axios
            .post(
              Config.CoursesBackend +
                "courses/" +
                courseId +
                "/tasks/" +
                blockId +
                "/check",
              taskInputs
            )
            .then((response) => {
              console.log(response.data);
              setTaskPointsSequence(response.data);
            })
            .catch((error) => console.log(error));
        }}
      >
        отправить на проверку
      </Button>
    </form>
  );
}

export default Tasks;
