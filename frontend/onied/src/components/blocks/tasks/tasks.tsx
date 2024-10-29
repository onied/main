import { useEffect, useState } from "react";
import GeneralTask from "./generalTask";
import classes from "./tasks.module.css";
import api from "@onied/config/axios";
import { useAppSelector } from "@onied/hooks";
import Button from "@onied/components/general/button/button";
import CustomBeatLoader from "@onied/components/general/customBeatLoader";
import { Task, TaskPointsResponse, UserInputRequest } from "@onied/types/task";
import { TasksBlock } from "@onied/types/block";
import { Module } from "@onied/types/course";

type props = {
  courseId: number;
  blockId: number;
};

function Tasks({ courseId, blockId }: props) {
  const hierarchyState = useAppSelector((state) => state.hierarchy);

  const [tasks, setTasks] = useState<TasksBlock>();
  const [found, setFound] = useState<boolean>();
  const [taskInputs, setTaskInputs] = useState<UserInputRequest[]>([]);
  const [reloadNeeded, setReloadNeeded] = useState<number>(0);
  const [taskPointsSequence, setTaskPointsSequence] = useState<
    TaskPointsResponse[] | null
  >();

  const handleChange = (inputIndex: number, input: any) => {
    const newTaskInputs = [...taskInputs];
    newTaskInputs[inputIndex] = input;
    console.log(newTaskInputs);
    setTaskInputs(newTaskInputs);
  };

  useEffect(() => {
    api
      .get("courses/" + courseId + "/tasks/" + blockId + "/points")
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
    api
      .get("courses/" + courseId + "/tasks/" + blockId)
      .then((response) => {
        console.log(response.data);
        setFound(true);
        setTasks(response.data);
        setTaskInputs(
          response.data.tasks.map((task: Task) => {
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
  }, [courseId, blockId, reloadNeeded]);

  if (found == null || taskPointsSequence == null) return <CustomBeatLoader />;
  if (!found) return <></>;

  return (
    <form className={classes.tasksContainer} action="post">
      <h2>{tasks!.title}</h2>
      {tasks!.tasks.map((task, index) => {
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
        onClick={(e: any) => {
          e.preventDefault();
          console.log(taskInputs);

          setTaskPointsSequence(undefined);
          api
            .post(
              "courses/" + courseId + "/tasks/" + blockId + "/check",
              taskInputs
            )
            .then((response) => {
              const newTaskPoints = response.data;
              console.log(response.data);
              setTaskPointsSequence(newTaskPoints);
              setReloadNeeded(reloadNeeded + 1);

              if (
                tasks!.tasks.every(
                  (t, index) => t.maxPoints == newTaskPoints[index]?.points
                )
              ) {
                const moduleId = hierarchyState.hierarchy.modules.find(
                  (module: Module) => module.blocks.some((b) => b.id == blockId)
                ).id;
                const block =
                  hierarchyState.hierarchy.modules[moduleId].blocks[blockId];
                block.completed = true;
                console.log(hierarchyState.hierarchy);
              }
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
