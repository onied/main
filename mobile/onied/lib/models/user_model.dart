import 'package:onied_mobile/models/enums/gender.dart';

class UserModel {
  String firstName;
  String lastName;
  Gender gender;
  String email;
  String? avatar;
  UserModel({
    required this.firstName,
    required this.lastName,
    required this.gender,
    required this.email,
    this.avatar,
  });

  String get fullName => '$firstName $lastName';

  UserModel copyWith({
    String? firstName,
    String? lastName,
    Gender? gender,
    String? email,
    String? avatar,
  }) {
    return UserModel(
      firstName: firstName ?? this.firstName,
      lastName: lastName ?? this.lastName,
      gender: gender ?? this.gender,
      email: email ?? this.email,
      avatar: avatar ?? this.avatar,
    );
  }

  factory UserModel.fromJson(Map<String, dynamic> json) {
    return UserModel(
      firstName: json['firstName'] as String,
      lastName: json['lastName'] as String,
      gender: Gender.values[json['gender'] as int],
      email: json['email'] as String,
      avatar: json['avatar'] as String?,
    );
  }
}
