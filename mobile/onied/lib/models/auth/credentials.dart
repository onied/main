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
        "tokenType": "Bearer",
        'accessToken': String accToken,
        'expiresIn': int dt,
        'refreshToken': String refToken,
      } =>
        Credentials(
          accessToken: accToken,
          expiresIn: DateTime.now().add(Duration(seconds: dt)),
          refreshToken: refToken,
        ),
      _ => throw const FormatException('Failed to load credentials.'),
    };
  }
}
