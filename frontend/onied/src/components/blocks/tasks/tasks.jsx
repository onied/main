import Button from "../../general/button/button";
import GeneralTask from "./generalTask";
import taskType from "./taskType";
import classes from "./tasks.module.css";

function Tasks() {
  const tasks = {
    title: "Заголовок блока с заданиями",
    tasks: [
      {
        id: 1,
        title: "1. Что произошло на пло́щади Тяньаньмэ́нь в 1989 году?",
        type: taskType.MULTIPLE_ANSWERS,
        variants: [
          { id: 1, description: "Ничего" },
          { id: 2, description: "Ничего" },
          { id: 3, description: "Ничего" },
          { id: 4, description: "Ничего" },
        ],
        pointInfo: { completed: false },
      },
      {
        id: 2,
        title: "2. Чипи чипи чапа чапа дуби дуби даба даба?",
        type: taskType.SINGLE_ANSWER,
        variants: [
          { id: 5, description: "Чипи чипи" }, // variant_id is globally unique
          { id: 6, description: "Чапа чапа" },
          { id: 7, description: "Дуби дуби" },
          { id: 8, description: "Даба даба" },
        ],
        pointInfo: { completed: true, points: 0, maxPoints: 1 },
      },
      {
        id: 3,
        title: "3. Кто?",
        type: taskType.INPUT_ANSWER,
        pointInfo: { completed: true, points: 5, maxPoints: 5 },
      },
      {
        id: 4,
        title: "4. Напишите эссе на тему: “Как я провел лето”",
        type: taskType.REVIEW_ANSWER,
        pointInfo: { completed: false },
      },
    ],
  };
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
