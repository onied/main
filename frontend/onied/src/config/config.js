class Config {
  static BaseURL = "http://localhost:5288/api/v1/";
  static ClientId = "51882579";
  static RedirectUrl = "https://onied.com/oauth-redirect";
  static VkAuthorizationUrl = "https://oauth.vk.com/authorize";
  static MapboxApiKey = import.meta.env.VITE_MAPBOX_API_KEY;
}

export default Config;
