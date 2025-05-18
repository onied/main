import 'package:onied_mobile/models/enums/gender.dart';

class UpdateUserModel {
  String firstName;
  String lastName;
  Gender gender;

  UpdateUserModel({
    required this.firstName,
    required this.lastName,
    required this.gender,
  });
}
