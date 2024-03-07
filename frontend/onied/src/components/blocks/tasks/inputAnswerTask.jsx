import LineEdit from "../../general/lineedit/lineedit";

function InputAnswerTask({ task }) {
  return (
    <>
      <LineEdit name={task.id}></LineEdit>
    </>
  );
}

export default InputAnswerTask;
