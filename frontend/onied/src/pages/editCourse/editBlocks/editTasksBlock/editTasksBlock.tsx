import classes from "./editVideoBlock.module.css";
import { useProfile } from "../../../../hooks/profile/useProfile";
import { Navigate } from "react-router-dom";
import EditTasksBlockComponent from "../../../../components/editCourse/editBlocks/editTasksBlock/editTasksBlock";

function EditTasksBlock() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <div className={classes.container}>
        <EditTasksBlockComponent />
      </div>
    </>
  );
}

export default EditTasksBlock;
