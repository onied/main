import { Navigate, Route, Routes } from "react-router-dom";
import { useProfile } from "../../hooks/profile/useProfile";
import CoursePurchase from "../../components/purchase/coursePurchase";
import SubscriptionPurchase from "../../components/purchase/subscriptionPurchase";
import CertificatePurchase from "../../components/purchase/certificatePurchase";

function PurchasePage() {
  const NotFound = <h2>Страница не найдена</h2>;

  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;

  return (
    <>
      <Routes>
        <Route path="/*" element={NotFound} />
        <Route path="/course/:courseId" element={<CoursePurchase />} />
        <Route
          path="/subscription/:subscriptionId"
          element={<SubscriptionPurchase />}
        />
        <Route
          path="/certificate/:courseId"
          element={<CertificatePurchase />}
        />
      </Routes>
    </>
  );
}

export default PurchasePage;
