import classes from "./editTaskBlock.module.css";
import { useProfile } from "../../../../hooks/profile/useProfile";
import { Navigate, useParams } from "react-router-dom";
import EditTasksBlockComponent from "../../../../components/editCourse/editBlocks/editTasksBlock";

function EditTasksBlock() {
  const { courseId, blockId } = useParams();
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <div className={classes.container}>
        <EditTasksBlockComponent
          courseId={Number.parseInt(courseId!)}
          blockId={Number.parseInt(blockId!)}
        />
      </div>
    </>
  );
}

export default EditTasksBlock;
