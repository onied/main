class Config {
  static const backendUrl = "http://localhost:8080";
  static const graphQlEndpoint = "http://10.0.2.2:5288/graphql";
  static const String clientId = '51882579';
  static const String redirectUri = 'onied://auth';
  static const String authUrl =
      'https://id.vk.com/auth?app_id=$clientId&redirect_uri=$redirectUri&response_type=code';
}
