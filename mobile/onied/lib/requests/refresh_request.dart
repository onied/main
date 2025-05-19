class RefreshRequest {
  final String refreshToken;

  const RefreshRequest({required this.refreshToken});

  Map<String, dynamic> toJson() {
    return <String, dynamic>{"refreshToken": refreshToken};
  }
}
