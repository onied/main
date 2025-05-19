import 'package:onied_mobile/models/enums/gender.dart';

class ProfileChangedRequest {
  final String firstName;
  final String lastName;
  final Gender gender;

  const ProfileChangedRequest({
    required this.firstName,
    required this.lastName,
    required this.gender,
  });

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "firstName": firstName,
      "lastName": lastName,
      "gender": gender.index,
    };
  }
}
