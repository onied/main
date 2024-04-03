import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import api from "../../../../config/axios";
import EditSingleAnswerTask from "../../editTasks/editSingleAnswerTask/editSingleAnswerTask";
import { TasksBlock } from "../../../../types/block";
import { Task, TaskType } from "../../../../types/task";
import { BeatLoader } from "react-spinners";

function EditTasksBlockComponent() {
  const { courseId, blockId } = useParams();
  const [loading, setLoading] = useState(false);
  const [currentBlock, setCurrentBlock] = useState<
    TasksBlock | undefined | null
  >(undefined);

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

  const task: Task = {
    id: 145,
    title: "типовое задание",
    taskType: TaskType.ManualReview,
    maxPoints: 1,
    variants: null,
  };

  return (
    <>
      <h2>{currentBlock?.title}</h2>
      <EditSingleAnswerTask task={task} />
    </>
  );
}

export default EditTasksBlockComponent;
