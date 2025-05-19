import 'package:equatable/equatable.dart';
import 'package:onied_mobile/models/enums/gender.dart';

class UserModel extends Equatable {
  final String firstName;
  final String lastName;
  final Gender gender;
  final String email;
  final String? avatar;

  const UserModel({
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

  @override
  List<Object?> get props => [firstName, lastName, gender, email, avatar];

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
