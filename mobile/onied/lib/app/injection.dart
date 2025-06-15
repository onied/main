import 'package:get_it/get_it.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:onied_mobile/providers/authorization_provider.dart';
import 'package:onied_mobile/providers/courses_provider.dart';
import 'package:onied_mobile/providers/user_provider.dart';
import 'package:onied_mobile/providers/vk_auth_provider.dart';
import 'package:onied_mobile/repositories/chat_repository.dart';
import 'package:onied_mobile/repositories/user_repository.dart';
import 'package:onied_mobile/services/graphql_service.dart';

final getIt = GetIt.instance;

void setupDependencies() {
  getIt.registerSingleton<FlutterSecureStorage>(const FlutterSecureStorage());

  getIt.registerSingleton<VKAuthProvider>(VKAuthProvider());
  getIt.registerSingleton<UserProvider>(UserProvider());
  getIt.registerSingleton<AuthorizationProvider>(
    AuthorizationProvider(flutterSecureStorage: getIt<FlutterSecureStorage>()),
  );

  getIt.registerSingleton<GraphQlService>(
    GraphQlService(authorizationProvider: getIt<AuthorizationProvider>()),
  );

  getIt.registerSingleton<CourseProvider>(
    CourseProvider(getIt<GraphQlService>(), getIt<AuthorizationProvider>()),
  );

  getIt.registerSingleton<UserRepository>(
    UserRepository(
      authorizationProvider: getIt<AuthorizationProvider>(),
      userProvider: getIt<UserProvider>(),
    ),
  );
  getIt.registerSingleton<ChatRepository>(
    ChatRepository(authorizationProvider: getIt<AuthorizationProvider>()),
  );
}
