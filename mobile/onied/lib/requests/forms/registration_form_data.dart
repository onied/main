class RegistrationFormData {
  final String name;
  final String surname;
  final int sex;
  final String email;
  final String password;

  const RegistrationFormData({
    required this.name,
    required this.surname,
    required this.sex,
    required this.email,
    required this.password,
  });

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "name": name,
      "surname": surname,
      "sex": sex,
      "email": email,
      "password": password,
    };
  }
}
