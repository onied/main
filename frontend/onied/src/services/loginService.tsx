import api from "../config/axios";

class LoginService {
  static interval: NodeJS.Timeout | null = null;

  static registerAutomaticRefresh() {
    LoginService.interval = setInterval(LoginService.refreshTokens, 600000);
    // refresh tokens every 10 minutes
  }

  static unregisterAutomaticRefresh() {
    if (LoginService.interval == null) return;
    clearInterval(LoginService.interval);
  }

  static storeTokens(
    accessToken: string,
    expiresIn: number,
    refreshToken: string
  ) {
    localStorage.setItem("access_token", accessToken);
    localStorage.setItem("refresh_token", refreshToken);
    var date = new Date();
    date.setSeconds(date.getSeconds() + expiresIn);
    localStorage.setItem("expires", date.toString());
    api.defaults.headers.common["Authorization"] = `Bearer ${accessToken}`;
  }

  static checkLoggedIn(): Boolean {
    const accessToken = localStorage.getItem("access_token");
    const refreshToken = localStorage.getItem("refresh_token");
    const expiresString = localStorage.getItem("expires");
    if (accessToken === null || refreshToken === null || expiresString === null)
      return false;
    const expires = new Date(localStorage.getItem("expires")!);
    if (expires <= new Date()) {
      LoginService.refreshTokens();
      return false;
    }
    return true;
  }

  static clearTokens() {
    localStorage.removeItem("access_token");
    localStorage.removeItem("refresh_token");
    localStorage.removeItem("expires");
    delete api.defaults.headers.common["Authorization"];
  }

  static refreshTokens() {
    const refreshToken = localStorage.getItem("refresh_token");
    if (refreshToken === null) return;
    console.log("Token refresh requested");
    api
      .post("refresh", {
        refreshToken: refreshToken,
      })
      .then((response) => {
        LoginService.storeTokens(
          response.data.accessToken,
          response.data.expiresIn,
          response.data.refreshToken
        );
      })
      .catch((_) => {
        LoginService.clearTokens();
      });
  }
}

export default LoginService;
