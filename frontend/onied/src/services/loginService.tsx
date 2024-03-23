import api from "../config/axios";

class LoginService {
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
      this.refreshTokens();
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
    api
      .post("refresh", {
        refreshToken: refreshToken,
      })
      .then((response) => {
        this.storeTokens(
          response.data.accessToken,
          response.data.expiresIn,
          response.data.refreshToken
        );
      })
      .catch((_) => {
        this.clearTokens();
      });
  }
}

export default LoginService;
