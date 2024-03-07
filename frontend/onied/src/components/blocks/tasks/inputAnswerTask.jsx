import { useState } from "react";
import LineEdit from "../../general/lineedit/lineedit";

function InputAnswerTask({ task }) {
  const [value, setValue] = useState("");
  return (
    <>
      <LineEdit
        name={task.id}
        value={value}
        onChange={(event) => setValue(event.target.value)}
      ></LineEdit>
    </>
  );
}

export default InputAnswerTask;
