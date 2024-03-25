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
import { useState } from "react";
import { Profile } from "./hooks/profile/profile";
import { ProfileContext } from "./hooks/profile/profileContext";
import ProfilePage from "./pages/profile/profile";
import ConfirmEmail from "./pages/accountManagement/confirmEmail/confirmEmail";

function App() {
  const [profile, setProfile] = useState<Profile | null>(
    {
      firstName: "Иван",
      lastName: "Иванов",
      gender: 0,
      avatarHref: "http://loremflickr.com/400/400",
      email: "test@example.com",
    }
    // null
  );
  LoginService.initialize();
  LoginService.registerAutomaticRefresh();
  return (
    <>
      <ProfileContext.Provider value={profile}>
        <Header></Header>
        <main>
          <Routes>
            <Route
              path="/course/:courseId/learn/*"
              element={<Course />}
            ></Route>
            <Route path="/course/:courseId" element={<Preview />}></Route>
            <Route path="/catalog" element={<Catalog />}></Route>
            <Route path="/register" element={<Register />}></Route>
            <Route path="/login" element={<Login />}></Route>
            <Route path="/login/2fa" element={<TwoFactor />}></Route>
            <Route path="/forgotPassword" element={<ForgotPassword />}></Route>
            <Route path="/resetPassword" element={<ResetPassword />}></Route>
            <Route path="/oauth-redirect" element={<OauthRedirect />}></Route>
            <Route path="/confirmEmail" element={<ConfirmEmail />}></Route>
            <Route path="/profile/*" element={<ProfilePage />}></Route>
          </Routes>
        </main>
      </ProfileContext.Provider>
    </>
  );
}

export default App;
