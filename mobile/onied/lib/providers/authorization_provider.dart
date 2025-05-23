import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:onied_mobile/models/auth/credentials.dart';

class AuthorizationProvider {
  final FlutterSecureStorage flutterSecureStorage;

  const AuthorizationProvider({required this.flutterSecureStorage});

  Future<void> setupCredentials(Credentials authData) async {
    await flutterSecureStorage.write(
      key: "accessToken",
      value: authData.accessToken,
    );
    await flutterSecureStorage.write(
      key: "refreshToken",
      value: authData.refreshToken,
    );
    await flutterSecureStorage.write(
      key: "expiresIn",
      value: authData.expiresIn.millisecondsSinceEpoch.toString(),
    );
  }

  Future<Credentials?> getCredentials() async {
    final accessToken = await flutterSecureStorage.read(key: "accessToken");
    final refreshToken = await flutterSecureStorage.read(key: "refreshToken");
    final expiresIn = await flutterSecureStorage.read(key: "expiresIn");

    if (accessToken == null || refreshToken == null || expiresIn == null) {
      return null;
    }

    return Credentials(
      accessToken: accessToken,
      refreshToken: refreshToken,
      expiresIn: DateTime.fromMillisecondsSinceEpoch(int.parse(expiresIn)),
    );
  }

  Future<bool> isAuthenticated() async {
    final expiresString = await flutterSecureStorage.read(key: "expiresIn");
    if (expiresString == null) {
      return false;
    }
    final expiresIn = int.tryParse(expiresString);
    if (expiresIn == null ||
        expiresIn < DateTime.now().millisecondsSinceEpoch) {
      return false;
    }
    return true;
  }
}
