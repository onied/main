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
        variants: ["Ничего", "Ничего", "Ничего", "Ничего"],
        pointInfo: { completed: false },
      },
      {
        id: 2,
        title: "2. Чипи чипи чапа чапа дуби дуби даба даба?",
        type: taskType.SINGLE_ANSWER,
        variants: ["Чипи чипи", "Чапа чапа", "Дуби дуби", "Даба даба"],
        pointInfo: { completed: true, points: 0, maxPoints: 1 },
      },
    ],
  };
  return (
    <div className={classes.tasksContainer}>
      <h2>{tasks.title}</h2>
      {tasks.tasks.map((task, index) => {
        return <GeneralTask task={task} key={index}></GeneralTask>;
      })}
    </div>
  );
}

export default Tasks;
