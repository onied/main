import { Navigate, Route, Routes } from "react-router-dom";
import CardContainer from "../../components/purchase/cardContainer/cardContainer";
import { useProfile } from "../../hooks/profile/useProfile";

function PurchasePage() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;

  return (
    <>
      <Routes>
        <Route path="/*" element={<Navigate to="/purchases/courses" />} />
        <Route path="/courses" element={<CardContainer />} />
        <Route path="/subscription" element={<></>} />
        <Route path="/certificate" element={<></>} />
      </Routes>
    </>
  );
}

export default PurchasePage;
