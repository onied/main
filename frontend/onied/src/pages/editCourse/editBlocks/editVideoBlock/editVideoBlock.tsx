import classes from "./editVideoBlock.module.css";
import EditVideoBlockComponent from "../../../../components/editCourse/editBlocks/editVideoBlock/editVideoBlock";
import { useProfile } from "../../../../hooks/profile/useProfile";
import { Navigate, useParams } from "react-router-dom";

function EditVideoBlock() {
  const { courseId, blockId } = useParams();
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <div className={classes.container}>
        <EditVideoBlockComponent
          courseId={Number.parseInt(courseId!)}
          blockId={Number.parseInt(blockId!)}
        />
      </div>
    </>
  );
}

export default EditVideoBlock;
