import MDEditor from "@uiw/react-md-editor";
import { useState } from "react";
import rehypeSanitize from "rehype-sanitize";

function ManualReviewTask() {
  const [value, setValue] = useState("");
  return (
    <>
      <MDEditor
        value={value}
        onChange={setValue}
        previewOptions={{
          rehypePlugins: [[rehypeSanitize]],
        }}
      />
    </>
  );
}

export default ManualReviewTask;
