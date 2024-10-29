import { useState } from "react";
import LineEdit from "../../general/lineedit/lineedit";
import { Task, TaskType, UserInputRequest } from "@onied/types/task";

type props = {
  task: Task;
  onChange: (request: UserInputRequest) => void;
};

function InputAnswerTask({ task, onChange }: props) {
  const [value, setValue] = useState("");

  const handleChange = (event: any) => {
    setValue(event.target.value);

    onChange({
      taskId: task.id,
      taskType: TaskType.InputAnswer,
      isDone: true,
      answer: event.target.value,
    });
  };

  return (
    <>
      <LineEdit name={task.id} value={value} onChange={handleChange}></LineEdit>
    </>
  );
}

export default InputAnswerTask;
