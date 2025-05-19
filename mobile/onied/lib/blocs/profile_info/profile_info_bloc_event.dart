import 'package:onied_mobile/models/user_model.dart';

abstract class ProfileInfoBlocEvent {}

class LoadUser extends ProfileInfoBlocEvent {}

class UpdateUserModel extends ProfileInfoBlocEvent {
  final UserModel user;

  UpdateUserModel({required this.user});
}

class SaveUserInfo extends ProfileInfoBlocEvent {
  final UserModel user;

  SaveUserInfo({required this.user});
}

class SaveUserAvatar extends ProfileInfoBlocEvent {
  final String? avatar;

  SaveUserAvatar({this.avatar});
}
