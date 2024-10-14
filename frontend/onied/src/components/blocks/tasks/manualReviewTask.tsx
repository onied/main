import MDEditor, { ContextStore } from "@uiw/react-md-editor";
import { useEffect, useState } from "react";
import rehypeSanitize from "rehype-sanitize";
import { Task, TaskType, UserInputRequest } from "@onied/types/task";

type props = {
  task: Task;
  onChange: (request: UserInputRequest) => void;
  initialValue: string;
};

function ManualReviewTask({ task, onChange, initialValue }: props) {
  const [value, setValue] = useState<string>("");

  // default library signature
  const handleChange = (
    text?: string, 
    _event?: React.ChangeEvent<HTMLTextAreaElement>, 
    _state?: ContextStore) => {
    setValue(text ?? "");
    onChange({
      taskId: task.id,
      taskType: TaskType.ManualReview,
      isDone: true,
      text: text,
    });
  };

  useEffect(() => {
    if (initialValue) handleChange(initialValue);
  }, [initialValue]);

  return (
    <>
      <MDEditor
        value={value}
        onChange={handleChange}
        previewOptions={{
          rehypePlugins: [[rehypeSanitize]],
        }}
      />
    </>
  );
}

export default ManualReviewTask;
