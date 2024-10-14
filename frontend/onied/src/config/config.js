class Config {
  static BaseURL = import.meta.env.VITE_API_URL ?? "http://localhost";
  static ClientId = import.meta.env.VITE_VK_CLIENT_ID ?? "123456";
  static RedirectUrl = import.meta.env.VITE_REDIRECT_URL ?? "http://localhost";
  static VkAuthorizationUrl =
    import.meta.env.VITE_VK_AUTH_URL ?? "http://localhost";
  static MapboxApiKey = import.meta.env.VITE_MAPBOX_API_KEY ?? "mapbox-api-key";
}

export default Config;
