import MDEditor from "@uiw/react-md-editor";
import { useState } from "react";
import rehypeSanitize from "rehype-sanitize";

function ManualReviewTask({ task, onChange }) {
  const [value, setValue] = useState("");

  const handleChange = (text) => {    
    setValue(text);
    onChange({ id: task.id, answer: text });
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
