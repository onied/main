import { useCallback, useState } from "react";
import LineEdit from "../../general/lineedit/lineedit";

function InputAnswerTask({ task, onChange }) {
  const [value, setValue] = useState("");

  const handleChange = (event) => {    
    setValue(event.target.value);
    
    onChange({
      id: task.id,
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
