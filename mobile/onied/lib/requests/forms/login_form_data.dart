class LoginFormData {
  final String email;
  final String password;
  final String? twoFactorCode;

  const LoginFormData({
    required this.email,
    required this.password,
    this.twoFactorCode,
  });

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "email": email,
      "password": password,
      if (twoFactorCode != null) "twoFactorCode": twoFactorCode,
    };
  }
}
