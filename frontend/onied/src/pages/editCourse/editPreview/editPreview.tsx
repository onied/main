import EditPreviewComponent from "../../../components/editCourse/editPreview/editPreview";
import classes from "../editPreview/editPreview.module.css";
import { Navigate } from "react-router-dom";
import { useProfile } from "../../../hooks/profile/useProfile";

function EditPreview() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <div className={classes.container}>
      <EditPreviewComponent />
    </div>
  );
}

export default EditPreview;
