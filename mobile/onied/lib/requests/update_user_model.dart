import 'package:onied_mobile/models/enums/gender.dart';

class UpdateUserModelRequest {
  String firstName;
  String lastName;
  Gender gender;

  UpdateUserModelRequest({
    required this.firstName,
    required this.lastName,
    required this.gender,
  });
}
