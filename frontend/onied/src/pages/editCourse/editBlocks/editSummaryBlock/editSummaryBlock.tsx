import EditSummaryBlockComponent from "../../../../components/editCourse/editBlocks/editSummaryBlock/editSummaryBlock";
import classes from "./editSummaryBlock.module.css";
import { useProfile } from "../../../../hooks/profile/useProfile";
import { Navigate } from "react-router-dom";

function EditSummaryBlock() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <div className={classes.summaryEditWrapper}>
        <EditSummaryBlockComponent />
      </div>
    </>
  );
}

export default EditSummaryBlock;
