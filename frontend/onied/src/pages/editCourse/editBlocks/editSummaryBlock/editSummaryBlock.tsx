import EditSummaryBlockComponent from "../../../../components/editCourse/editBlocks/editSummaryBlock/editSummaryBlock";
import classes from "./editSummaryBlock.module.css";
import { useProfile } from "../../../../hooks/profile/useProfile";
import { Navigate, useParams } from "react-router-dom";

function EditSummaryBlock() {
  const { courseId, blockId } = useParams();
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <EditSummaryBlockComponent
        courseId={Number.parseInt(courseId!)}
        blockId={Number.parseInt(blockId!)}
      />
    </>
  );
}

export default EditSummaryBlock;
