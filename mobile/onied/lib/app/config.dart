class Config {
  static const backendUrl = "http://192.168.31.114:8080";

  static const String clientId = '51882579';
  static const String redirectUri = 'onied://auth';
  static const String authUrl =
      'https://id.vk.com/auth?app_id=$clientId&redirect_uri=$redirectUri&response_type=code';
}
