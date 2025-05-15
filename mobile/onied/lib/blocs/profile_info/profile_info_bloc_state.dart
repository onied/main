import 'package:onied_mobile/models/user_model.dart';

abstract class ProfileInfoBlocState {}

class LoadingState extends ProfileInfoBlocState {}

class ErrorState extends ProfileInfoBlocState {
  String errorMessage;

  ErrorState({required this.errorMessage});
}

class LoadedState extends ProfileInfoBlocState {
  final UserModel user;

  LoadedState({required this.user});
}
