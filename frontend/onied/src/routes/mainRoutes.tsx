import Header from "@onied/components/header/header";
import Landing from "@onied/pages/landing/landing";
import Catalog from "@onied/pages/catalog/catalog.jsx";
import SubscriptionsPreview from "@onied/pages/subscriptions/subscriptionsPreview";
import ProfilePage from "@onied/pages/profile/profile";
import PurchasePage from "@onied/pages/purchase/purchase";
import OrderCertificatePage from "@onied/pages/certificates/orderCertificate";
import ConfirmEmail from "@onied/pages/accountManagement/confirmEmail/confirmEmail";
import ForgotPassword from "@onied/pages/accountManagement/forgotPassword/forgotPassword";
import Login from "@onied/pages/accountManagement/login/login";
import Register from "@onied/pages/accountManagement/register/register";
import ResetPassword from "@onied/pages/accountManagement/resetPassword/resetPassword";
import TwoFactor from "@onied/pages/accountManagement/twoFactorAuth/twoFactor";
import OauthRedirect from "@onied/pages/oauthRedirect/oauthRedirect";
import CourseRoutes from "./courseRoutes";
import TeachingRoutes from "./teachingRoutes";

import { Route, Routes } from "react-router-dom";

export default function mainRoutes() {
    return <>
        <Header />
        <main>
            <Routes>
                <Route path="/" element={<Landing />} />
                <Route path="/catalog" element={<Catalog />} />

                <Route path="/course/*" element={<CourseRoutes />} />

                <Route path="/register" element={<Register />} />
                <Route path="/login" element={<Login />} />
                <Route path="/forgotPassword" element={<ForgotPassword />} />
                <Route path="/login/2fa" element={<TwoFactor />} />
                <Route path="/resetPassword" element={<ResetPassword />} />
                <Route path="/oauth-redirect" element={<OauthRedirect />} />
                <Route path="/confirmEmail" element={<ConfirmEmail />} />

                <Route path="/teaching/*" element={<TeachingRoutes />} />

                <Route path="/subscriptions" element={<SubscriptionsPreview />} />
                <Route path="/profile/*" element={<ProfilePage />} />
                <Route path="/purchases/*" element={<PurchasePage />} />
                <Route path="/certificates/:courseId" element={<OrderCertificatePage />} />
            </Routes>
        </main>
    </>
}