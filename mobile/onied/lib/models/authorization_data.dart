class AuthorizationData {
  final String accessToken;
  final DateTime expiresIn;
  final String refreshToken;

  const AuthorizationData({
    required this.accessToken,
    required this.expiresIn,
    required this.refreshToken
  });

  factory AuthorizationData.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'accessToken': String accToken,
        'expiresIn': DateTime dt,
        'refreshToken': String refToken
      } => AuthorizationData(
          accessToken: accToken,
          expiresIn: dt,
          refreshToken: refToken
      ),
      _ => throw const FormatException('Failed to load album.'),
    };
  }
}
