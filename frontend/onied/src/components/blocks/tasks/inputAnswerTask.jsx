import { useState } from "react";
import LineEdit from "../../general/lineedit/lineedit";
import taskType from "./taskType";

function InputAnswerTask({ task, onChange }) {
  const [value, setValue] = useState("");

  const handleChange = (event) => {    
    setValue(event.target.value);
    
    onChange({
      taskId: task.id,
      taskType: taskType.INPUT_ANSWER,
      isDone: true,
      answer: event.target.value,
    });
  }

  return (
    <>
      <LineEdit
        name={task.id}
        value={value}
        onChange={handleChange}
      ></LineEdit>
    </>
  );
}

export default InputAnswerTask;
