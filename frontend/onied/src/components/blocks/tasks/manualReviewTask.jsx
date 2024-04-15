import MDEditor from "@uiw/react-md-editor";
import { useEffect, useState } from "react";
import rehypeSanitize from "rehype-sanitize";
import taskType from "./taskType";

function ManualReviewTask({ task, onChange, initialValue }) {
  const [value, setValue] = useState("");

  const handleChange = (text) => {
    setValue(text);
    onChange({
      taskId: task.id,
      taskType: taskType.REVIEW_ANSWER,
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
