import classes from "./editVideoBlock.module.css";
import EditVideoBlockComponent from "../../../../components/editCourse/editBlocks/editVideoBlock/editVideoBlock";
import { useProfile } from "../../../../hooks/profile/useProfile";
import { Navigate } from "react-router-dom";

function EditVideoBlock() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <div className={classes.container}>
        <EditVideoBlockComponent />
      </div>
    </>
  );
}

export default EditVideoBlock;
