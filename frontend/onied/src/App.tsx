import "./App.css";

import LoginService from "@onied/services/loginService";
import ProfileService from "@onied/services/profileService";

import { Profile } from "@onied/hooks/profile/profile";
import { ProfileContext } from "@onied/hooks/profile/profileContext";
import { LoadingContext } from "@onied/hooks/profile/loadingContext";
import CustomBeatLoader from "@onied/components/general/customBeatLoader";

import MainRoutes from "@onied/routes/mainRoutes";
import OperatorsPage from "@onied/pages/operators/operatorsPage";

import { useEffect, useState } from "react";
import { Route, Routes } from "react-router-dom";

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

  if (loading) return <CustomBeatLoader />;

  return (
    <>
      <ProfileContext.Provider value={profile}>
        <LoadingContext.Provider value={loading}>
          <Routes>
            <Route path="/operators" element={<OperatorsPage />} />
            <Route path="*" element={<MainRoutes />} />
          </Routes>
        </LoadingContext.Provider>
      </ProfileContext.Provider>
    </>
  );
}

export default App;
