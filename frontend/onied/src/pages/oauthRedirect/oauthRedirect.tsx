import { useEffect, useState } from "react";
import { Navigate, useSearchParams } from "react-router-dom";
import api from "../../config/axios";
import LoginService from "../../services/loginService";
import ProfileService from "../../services/profileService";
import CustomBeatLoader from "../../components/general/customBeatLoader";

function OauthRedirect() {
  const [loginResult, setLoginResult] = useState<Boolean | undefined>();
  const [searchParams, _] = useSearchParams();
  const code = searchParams.get("code");
  const redirectUri = window.location.href.split("?")[0];
  useEffect(() => {
    if (code == null) return;
    api
      .post("signinVk", {
        code: code,
        redirectUri: redirectUri,
      })
      .then((response) => {
        LoginService.storeTokens(
          response.data.accessToken,
          response.data.expiresIn,
          response.data.refreshToken
        );
        ProfileService.fetchProfile();
        setLoginResult(true);
      })
      .catch((error) => {
        console.log(error);
        setLoginResult(false);
      });
  }, [code]);
  if (code == null) return <Navigate to={"/login"}></Navigate>;
  if (loginResult != undefined) return <Navigate to="/"></Navigate>;
  return <CustomBeatLoader />;
}

export default OauthRedirect;
