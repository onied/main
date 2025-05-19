import 'package:onied_mobile/models/enums/auth_status.dart';
import 'package:onied_mobile/models/auth/credentials.dart';

class AuthorizationData {
  final AuthStatus authStatus;
  final Credentials? credentials;

  const AuthorizationData({required this.authStatus, this.credentials});
}
