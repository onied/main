import { Navigate, Routes, Route } from "react-router-dom";
import { useProfile } from "../../hooks/profile/useProfile";
import TeachingHeader from "../../components/teaching/teachingHeader/teachingHeader";
import classes from "./teaching.module.css";
import TeachingAuthored from "../../components/teaching/authored";
import TeachingModerated from "../../components/teaching/moderated";
import TasksToCheck from "../../components/teaching/tasksToCheck/tasksToCheck";

function TeachingPage() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;
  return (
    <>
      <div className={classes.container}>
        <TeachingHeader></TeachingHeader>
        <Routes>
          <Route path="/" element={<TeachingAuthored />} />
          <Route path="/moderating" element={<TeachingModerated />} />
          <Route path="*" element={<Navigate to="/teaching" />} />
          <Route path="/check" element={<TasksToCheck />} />
        </Routes>
      </div>
    </>
  );
}

export default TeachingPage;
