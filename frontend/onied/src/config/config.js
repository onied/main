class Config {
  static BaseURL = import.meta.env.VITE_API_URL;
  static ClientId = import.meta.env.VITE_VK_CLIENT_ID;
  static RedirectUrl = import.meta.env.VITE_REDIRECT_URL;
  static VkAuthorizationUrl = import.meta.env.VITE_VK_AUTH_URL;
  static MapboxApiKey = import.meta.env.VITE_MAPBOX_API_KEY;
}

export default Config;
