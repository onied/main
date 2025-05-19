import 'package:equatable/equatable.dart';
import 'package:onied_mobile/models/user_model.dart';

abstract class ProfileInfoBlocState {}

class LoadingState extends ProfileInfoBlocState {}

class ErrorState extends ProfileInfoBlocState {
  String errorMessage;

  ErrorState({required this.errorMessage});
}

class LoadedState extends ProfileInfoBlocState with EquatableMixin {
  final UserModel user;

  LoadedState({required this.user});

  @override
  List<Object?> get props => [user];
}
