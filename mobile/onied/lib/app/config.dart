class Config {
  static const String ChatGrpcHost = "192.168.31.114";
  static const int ChatGrpcPort = 7191;
  static const backendUrl = "http://192.168.31.114:5288/api/v1";
  static const graphQlEndpoint = "http://192.168.31.114:5288/graphql";
  static const String clientId = '51882579';
  static const String redirectUri = 'onied://auth';
  static const String authUrl =
      'https://id.vk.com/auth?app_id=$clientId&redirect_uri=$redirectUri&response_type=code';
}
