import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import api from "../../../../config/axios";
import { TasksBlock } from "../../../../types/block";
import { SingleAnswerTask, Task } from "../../../../types/task";
import { BeatLoader } from "react-spinners";
import EditTask from "../../editTasks/editTask";
import ButtonGoBack from "../../../general/buttonGoBack/buttonGoBack";
import classes from "./index.module.css";

function EditTasksBlockComponent() {
  const navigator = useNavigate();
  const { courseId, blockId } = useParams();
  const [loading, setLoading] = useState(false);
  const [currentBlock, setCurrentBlock] = useState<
    TasksBlock | undefined | null
  >(undefined);

  const [task, setTask] = useState<Task>({
    id: 1,
    taskType: 1,
    title: "1. Что произошло на пло́щади Тяньаньмэ́нь в 1989 году?",
    maxPoints: 1,
    variants: [
      {
        id: 1,
        description: "Ничего 1",
      },
      {
        id: 2,
        description: "Ничего 2",
      },
      {
        id: 3,
        description: "Ничего 3",
      },
      {
        id: 4,
        description: "Ничего 4",
      },
    ],
    rightVariant: 1,
  } as Task);

  const notFound = <h1 style={{ margin: "3rem" }}>Курс или блок не найден.</h1>;

  useEffect(() => {
    const parsedCourseId = Number(courseId);
    const parsedBlockId = Number(blockId);
    if (isNaN(parsedCourseId) || isNaN(parsedBlockId)) {
      setLoading(false);
      setCurrentBlock(null);
      return;
    }

    setLoading(true);
    api
      .get("courses/" + courseId + "/tasks/" + blockId)
      .then((response) => {
        console.log(response.data);
        setLoading(false);
        setCurrentBlock(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setLoading(false);
          setCurrentBlock(null);
        }
      });
  }, []);

  if (loading) return <BeatLoader />;

  if (!loading && currentBlock === null) return notFound;

  const handleChange = (id: number, task: Task) => {
    console.log(task);
    setTask(task);
  };

  return (
    <div className={classes.container}>
      <ButtonGoBack
        onClick={() => navigator("../../hierarchy", { relative: "path" })}
      >
        ⟵ к редактированию иерархии
      </ButtonGoBack>
      <h2>{currentBlock?.title}</h2>
      <EditTask task={task} onChange={handleChange} />
    </div>
  );
}

export default EditTasksBlockComponent;
