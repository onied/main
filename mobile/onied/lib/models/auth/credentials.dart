class Credentials {
  final String accessToken;
  final DateTime expiresIn;
  final String refreshToken;

  const Credentials({
    required this.accessToken,
    required this.expiresIn,
    required this.refreshToken,
  });

  factory Credentials.fromJson(Map<String, dynamic> json) {
    return switch (json) {
      {
        'accessToken': String accToken,
        'expiresIn': DateTime dt,
        'refreshToken': String refToken,
      } =>
        Credentials(
          accessToken: accToken,
          expiresIn: dt,
          refreshToken: refToken,
        ),
      _ => throw const FormatException('Failed to load album.'),
    };
  }
}
