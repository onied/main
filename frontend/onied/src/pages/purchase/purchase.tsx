import { Navigate, Route, Routes } from "react-router-dom";
import { useProfile } from "../../hooks/profile/useProfile";
import CoursePurchase from "../../components/purchase/coursePurchase";

function PurchasePage() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;

  return (
    <>
      <Routes>
        <Route path="/*" element={<Navigate to="/purchases/course" />} />
        <Route path="/course/:courseId" element={<CoursePurchase />} />
        <Route path="/subscription" element={<></>} />
        <Route path="/certificate" element={<></>} />
      </Routes>
    </>
  );
}

export default PurchasePage;
