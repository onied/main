import { Navigate, Route, Routes } from "react-router-dom";
import { useProfile } from "../../hooks/profile/useProfile";
import CoursePurchase from "../../components/purchase/coursePurchase";
import SubscriptionPurchase from "../../components/purchase/subscriptionPurchase";
import CertificatePurchase from "../../components/purchase/certificatePurchase";
import NotFound from "../../components/general/responses/notFound/notFound";

function PurchasePage() {
  const [profile, loading] = useProfile();
  if (profile == null && !loading) return <Navigate to="/login"></Navigate>;

  return (
    <>
      <Routes>
        <Route path="/*" element={NotFound("Страница не найдена")} />
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
