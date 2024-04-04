import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../../../config/axios";
import { Task, TaskType } from "../../../../types/task";
import { BeatLoader } from "react-spinners";
import EditTask from "../../editTasks";
import ButtonGoBack from "../../../general/buttonGoBack/buttonGoBack";
import classes from "./index.module.css";
import { TasksBlock } from "../../../../types/block";
import TrashButton from "../../../general/trashButton";
import Button from "../../../general/button/button";

function EditTasksBlockComponent({
  courseId,
  blockId,
}: {
  courseId: number;
  blockId: number;
}) {
  const navigator = useNavigate();
  const [loading, setLoading] = useState(true);
  const [currentBlock, setCurrentBlock] = useState<
    TasksBlock | undefined | null
  >(undefined);

  const notFound = <h1 style={{ margin: "3rem" }}>Курс или блок не найден.</h1>;

  useEffect(() => {
    setCurrentBlock({
      id: 5,
      title: "Заголовок блока с заданиями",
      blockType: 3,
      isCompleted: false,
      tasks: [
        {
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
          rightVariants: [1, 3],
        },
        {
          id: 2,
          taskType: 0,
          title: "2. Чипи чипи чапа чапа дуби дуби даба даба?",
          maxPoints: 1,
          variants: [
            {
              id: 5,
              description: "Чипи чипи",
            },
            {
              id: 6,
              description: "Чапа чапа",
            },
            {
              id: 7,
              description: "Дуби дуби",
            },
            {
              id: 8,
              description: "Даба даба",
            },
          ],
          rightVariant: 5,
        },
        {
          id: 3,
          taskType: 2,
          title: "3. Кто?",
          maxPoints: 5,
          answers: [
            {
              id: 5,
              description: "Чипи чипи",
            },
            {
              id: 6,
              description: "Чапа чапа",
            },
            {
              id: 7,
              description: "Дуби дуби",
            },
            {
              id: 8,
              description: "Даба даба",
            },
          ],
          isNumber: false,
          checkAccuracy: null,
          checkRegister: false,
        },
        {
          id: 4,
          taskType: 3,
          title: "4. Напишите эссе на тему: “Как я провел лето”",
          maxPoints: 1,
        },
      ],
    });
    setLoading(false);
  }, []);

  // useEffect(() => {
  //   const parsedCourseId = Number(courseId);
  //   const parsedBlockId = Number(blockId);
  //   if (isNaN(parsedCourseId) || isNaN(parsedBlockId)) {
  //     setLoading(false);
  //     setCurrentBlock(null);
  //     return;
  //   }

  //   setLoading(true);
  //   api
  //     .get("courses/" + courseId + "/tasks/" + blockId)
  //     .then((response) => {
  //       console.log(response.data);
  //       setLoading(false);
  //       setCurrentBlock(response.data);
  //     })
  //     .catch((error) => {
  //       console.log(error);

  //       if ("response" in error && error.response.status == 404) {
  //         setLoading(false);
  //         setCurrentBlock(null);
  //       }
  //     });
  // }, []);

  if (loading) return <BeatLoader />;

  if (!loading && currentBlock === null) return notFound;

  const handleChange = (task: Task) => {
    console.log(task);

    const taskIndex = currentBlock!.tasks.findIndex((t) => task.id == t.id);
    const newTasks = [...currentBlock!.tasks];
    newTasks[taskIndex] = task;

    const newCurrentBlock = {
      ...currentBlock!,
      tasks: newTasks,
    };

    console.log(newCurrentBlock);
    setCurrentBlock(newCurrentBlock);
  };

  const saveChanges = () => {};

  const addTask = () => {
    const newTask: Task = {
      id:
        currentBlock!.tasks.length > 0
          ? currentBlock!.tasks[currentBlock!.tasks.length - 1].id + 1
          : 1,
      title: "",
      taskType: TaskType.ManualReview,
      maxPoints: 0,
    };
    const newTasks = [...currentBlock!.tasks, newTask];

    const newCurrentBlock = {
      ...currentBlock!,
      tasks: newTasks,
    };

    console.log(newCurrentBlock);
    setCurrentBlock(newCurrentBlock);
  };

  const removeTask = (taskId: number) => {
    const newTasks = currentBlock!.tasks.filter((t) => taskId != t.id);

    const newCurrentBlock = {
      ...currentBlock!,
      tasks: newTasks,
    };

    console.log(newCurrentBlock);
    setCurrentBlock(newCurrentBlock);
  };

  return (
    <div className={classes.container}>
      <ButtonGoBack
        onClick={() => navigator("../hierarchy", { relative: "path" })}
      >
        ⟵ к редактированию иерархии
      </ButtonGoBack>
      <h2>{currentBlock?.title}</h2>
      {currentBlock!.tasks.map((task, index) => (
        <div key={task.id} className={classes.containerItem}>
          <span className={classes.taskNumber}>{index + 1}.</span>
          <TrashButton
            onClick={(event: any) => {
              event.preventDefault();
              removeTask(task.id);
            }}
          />
          <EditTask task={task} onChange={handleChange} />
        </div>
      ))}
      <div className={classes.containerItem}>
        <Button onClick={saveChanges} style={{ width: "100%" }}>
          Сохранить изменения
        </Button>
        <Button onClick={addTask} style={{ width: "100%" }}>
          Добавить задание
        </Button>
      </div>
    </div>
  );
}

export default EditTasksBlockComponent;
