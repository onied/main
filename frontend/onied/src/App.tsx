import "./App.css";
import Header from "./components/header/header";
import Course from "./pages/course/course";
import { Route, Routes } from "react-router-dom";
import Preview from "./pages/preview/preview";
import Catalog from "./pages/catalog/catalog.jsx";
import ForgotPassword from "./pages/accountManagement/forgotPassword/forgotPassword";
import Register from "./pages/accountManagement/register/register.jsx";
import Login from "./pages/accountManagement/login/login";
import TwoFactor from "./pages/accountManagement/twoFactorAuth/twoFactor.jsx";
import ResetPassword from "./pages/accountManagement/resetPassword/resetPassword";
import OauthRedirect from "./pages/oauthRedirect/oauthRedirect";
import LoginService from "./services/loginService";
import { useEffect, useState } from "react";
import { Profile } from "./hooks/profile/profile";
import { ProfileContext } from "./hooks/profile/profileContext";
import ProfilePage from "./pages/profile/profile";
import ConfirmEmail from "./pages/accountManagement/confirmEmail/confirmEmail";
import ProfileService from "./services/profileService";
import { LoadingContext } from "./hooks/profile/loadingContext";
import TeachingPage from "./pages/teaching/teaching";
import EditCourseHierarchy from "./pages/editCourse/editHierarchy/editHierarchy";
import EditBlock from "./pages/editCourse/EditBlock";
import EditPreview from "./pages/editCourse/editPreview/editPreview";
import CheckTask from "./pages/checkTasks/checkTask/checkTask";
import CreateCourse from "./pages/createCourse/createCourse";
import PurchasePage from "./pages/purchase/purchase";
import SubscriptionsPreview from "./pages/subscriptions/subscriptionsPreview";

function App() {
  const [profile, setProfile] = useState<Profile | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshingTokens, setRefreshingTokens] = useState(false);
  LoginService.initialize(setRefreshingTokens);
  LoginService.registerAutomaticRefresh();
  ProfileService.initialize(setProfile, setLoading);
  useEffect(() => {
    if (refreshingTokens) setLoading(true);
    else if (LoginService.checkLoggedIn()) ProfileService.fetchProfile();
    else setLoading(false);
  }, [refreshingTokens]);
  return (
    <>
      <ProfileContext.Provider value={profile}>
        <LoadingContext.Provider value={loading}>
          <Header></Header>
          <main>
            <Routes>
              <Route path="/course/create" element={<CreateCourse />}></Route>
              <Route
                path="/course/:courseId/learn/*"
                element={<Course />}
              ></Route>
              <Route
                path="/course/:courseId/edit/hierarchy"
                element={<EditCourseHierarchy />}
              ></Route>
              <Route
                path="/course/:courseId/edit/:blockId"
                element={<EditBlock />}
              ></Route>
              <Route path="/course/:courseId/edit" element={<EditPreview />} />
              <Route path="/course/:courseId" element={<Preview />}></Route>
              <Route path="/catalog" element={<Catalog />}></Route>
              <Route
                path="/subscriptions"
                element={<SubscriptionsPreview />}
              ></Route>
              <Route path="/register" element={<Register />}></Route>
              <Route path="/login" element={<Login />}></Route>
              <Route path="/login/2fa" element={<TwoFactor />}></Route>
              <Route
                path="/forgotPassword"
                element={<ForgotPassword />}
              ></Route>
              <Route path="/resetPassword" element={<ResetPassword />}></Route>
              <Route path="/oauth-redirect" element={<OauthRedirect />}></Route>
              <Route path="/confirmEmail" element={<ConfirmEmail />}></Route>
              <Route path="/profile/*" element={<ProfilePage />}></Route>
              <Route
                path="/teaching/check/:taskCheckId"
                element={<CheckTask />}
              ></Route>
              <Route path="/teaching/*" element={<TeachingPage />}></Route>
              <Route path="/purchases/*" element={<PurchasePage />}></Route>
            </Routes>
          </main>
        </LoadingContext.Provider>
      </ProfileContext.Provider>
    </>
  );
}

export default App;
