abstract class AuthorizationBlocState {}

class LoadingState extends AuthorizationBlocState {}

class AuthorizedState extends AuthorizationBlocState {}

class NotAuthorizedState extends AuthorizationBlocState {}

class RequiresTwoFactorState extends AuthorizationBlocState {}

class ErrorState extends AuthorizationBlocState {
  final String errorMessage;

  ErrorState({required this.errorMessage});
}
