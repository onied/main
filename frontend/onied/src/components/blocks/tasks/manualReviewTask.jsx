import MDEditor from "@uiw/react-md-editor";
import { useState } from "react";
import rehypeSanitize from "rehype-sanitize";
import taskType from "./taskType";

function ManualReviewTask({ task, onChange }) {
  const [value, setValue] = useState("");

  const handleChange = (text) => {    
    setValue(text);
    onChange({ 
      taskId: task.id,
      taskType: taskType.REVIEW_ANSWER, 
      text: text 
    });
  }

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
