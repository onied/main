abstract class AuthorizationBlocState {}

class LoadingState extends AuthorizationBlocState {}

class LoadedState extends AuthorizationBlocState {}

class ErrorState extends AuthorizationBlocState {
  final String errorMessage;

  ErrorState({required this.errorMessage});
}
