import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import api from "../../../../config/axios";

enum TaskType {
  SingleAnswer = 0,
  MultipleAnswers,
  InputAnswer,
  ManualReview,
}

enum BlockType {
  AnyBlock = 0,
  SummaryBlock,
  VideoBlock,
  TasksBlock,
}

type Answer = {
  id: number;
  description: string;
};

type Task = {
  id: number;
  title: string;
  taskType: TaskType;
  maxPoints: number;
  variants: Answer[] | null;
};

type TasksBlock = {
  id: number;
  title: string;
  blockType: BlockType;
  tasks: Task[];
};

function EditTasksBlockComponent() {
  const navigator = useNavigate();
  const { courseId, blockId } = useParams();
  const [courseAndBlockFound, setCourseAndBlockFound] = useState(false);
  const [currentBlock, setCurrentBlock] = useState<TasksBlock | undefined>();

  const parsedCourseId = Number(courseId);
  const parsedBlockId = Number(blockId);

  useEffect(() => {
    if (isNaN(parsedCourseId) || isNaN(parsedBlockId)) {
      setCourseAndBlockFound(false);
      return;
    }
    api
      .get("courses/" + courseId + "/tasks/" + blockId)
      .then((response) => {
        console.log(response.data);
        setCourseAndBlockFound(true);
        setCurrentBlock(response.data);
      })
      .catch((error) => {
        console.log(error);

        if ("response" in error && error.response.status == 404) {
          setCourseAndBlockFound(false);
        }
      });
  }, []);

  return <></>;
}

export default EditTasksBlockComponent;
