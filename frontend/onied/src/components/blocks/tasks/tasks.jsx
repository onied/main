import Button from "../../general/button/button";
import GeneralTask from "./generalTask";
import taskType from "./taskType";
import classes from "./tasks.module.css";
import BeatLoader from "react-spinners/BeatLoader";
import axios from "axios";
import Config from "../../../config/config";
import { useEffect, useState } from "react";

function Tasks({ courseId, blockId }) {
  const [tasks, setTasks] = useState();
  const [found, setFound] = useState();

  useEffect(() => {
    setFound(undefined);
    axios
      .get(
        Config.CoursesBackend +
          "courses/" +
          courseId +
          "/get_tasks_block/" +
          blockId
      )
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setTasks(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, [courseId, blockId]);

  if (found == null)
    return <BeatLoader color="var(--accent-color)"></BeatLoader>;
  if (!found) return <></>;

  return (
    <form className={classes.tasksContainer}>
      <h2>{tasks.title}</h2>
      {tasks.tasks.map((task, index) => {
        return <GeneralTask task={task} key={index}></GeneralTask>;
      })}
      <Button type="submit">отправить на проверку</Button>
    </form>
  );
}

export default Tasks;
