import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/blocs/profile_info/profile_info_bloc.dart';
import 'package:onied_mobile/blocs/profile_info/profile_info_bloc_event.dart';
import 'package:onied_mobile/blocs/profile_info/profile_info_bloc_state.dart';
import 'package:onied_mobile/components/avatar/avatar.dart';
import 'package:onied_mobile/components/button/button.dart';
import 'package:onied_mobile/models/enums/gender.dart';
import 'package:onied_mobile/providers/authorization_provider.dart';
import 'package:onied_mobile/providers/user_provider.dart';
import 'package:onied_mobile/repositories/user_repository.dart';

class ProfileInfoPage extends StatelessWidget {
  const ProfileInfoPage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) => ProfileInfoBloc(
            repository: UserRepository(
              authorizationProvider: AuthorizationProvider(
                flutterSecureStorage: FlutterSecureStorage(),
              ),
              userProvider: UserProvider(),
            ),
          )..add(LoadUser()),
      child: Scaffold(
        appBar: AppBar(
          title: Text(
            "Редактирование профиля",
            style: Theme.of(
              context,
            ).textTheme.titleLarge?.copyWith(color: Colors.white),
          ),
          backgroundColor: AppTheme.backgroundColorHeader,
          foregroundColor: AppTheme.accent,
        ),
        body: SingleChildScrollView(
          child: BlocBuilder<ProfileInfoBloc, ProfileInfoBlocState>(
            builder: (context, state) {
              return switch (state) {
                LoadingState() => const Center(
                  child: CircularProgressIndicator(),
                ),
                LoadedState(:final user) => Column(
                  spacing: 24,
                  children: [
                    Container(
                      decoration: BoxDecoration(color: Colors.grey[200]),
                      padding: const EdgeInsets.all(32.0),
                      child: Row(
                        crossAxisAlignment: CrossAxisAlignment.center,
                        mainAxisAlignment: MainAxisAlignment.start,
                        spacing: 16.0,
                        children: [
                          Avatar(
                            width: 80,
                            name: user.fullName,
                            url: user.avatar,
                          ),
                          Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            mainAxisAlignment: MainAxisAlignment.center,
                            spacing: 4.0,
                            children: [
                              Text(
                                user.fullName,
                                style: TextStyle(
                                  fontSize: 20,
                                  fontWeight: FontWeight.bold,
                                ),
                              ),
                              Text(
                                user.email,
                                style: TextStyle(
                                  fontSize: 16,
                                  color: Colors.grey,
                                ),
                              ),
                            ],
                          ),
                        ],
                      ),
                    ),
                    Container(
                      padding: const EdgeInsets.all(16.0),
                      child: Form(
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          spacing: 16,
                          children: [
                            Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              spacing: 4,
                              children: [
                                Text(
                                  'Имя',
                                  style: Theme.of(context).textTheme.bodyLarge,
                                ),
                                TextFormField(
                                  initialValue: user.firstName,
                                  onChanged: (value) {
                                    context.read<ProfileInfoBloc>().add(
                                      UpdateUserModel(
                                        user: user.copyWith(firstName: value),
                                      ),
                                    );
                                  },
                                  decoration: InputDecoration(
                                    hintText: "Имя",
                                    hintStyle: Theme.of(context)
                                        .textTheme
                                        .bodyMedium
                                        ?.copyWith(color: Colors.grey),
                                    border: OutlineInputBorder(),
                                  ),
                                ),
                              ],
                            ),

                            Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              spacing: 4,
                              children: [
                                Text(
                                  'Фамилия',
                                  style: Theme.of(context).textTheme.bodyLarge,
                                ),
                                TextFormField(
                                  initialValue: user.lastName,
                                  onChanged: (value) {
                                    context.read<ProfileInfoBloc>().add(
                                      UpdateUserModel(
                                        user: user.copyWith(lastName: value),
                                      ),
                                    );
                                  },
                                  decoration: InputDecoration(
                                    hintText: 'Фамилия',
                                    hintStyle: Theme.of(context)
                                        .textTheme
                                        .bodyMedium
                                        ?.copyWith(color: Colors.grey),
                                    border: OutlineInputBorder(),
                                  ),
                                ),
                              ],
                            ),

                            Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              spacing: 4,
                              children: [
                                Text(
                                  'Пол',
                                  style: Theme.of(context).textTheme.bodyLarge,
                                ),
                                Column(
                                  children:
                                      [
                                        (
                                          value: Gender.other,
                                          display: "Не указан",
                                        ),
                                        (
                                          value: Gender.male,
                                          display: "Мужской",
                                        ),
                                        (
                                          value: Gender.female,
                                          display: "Женский",
                                        ),
                                      ].map<Widget>((variant) {
                                        return RadioListTile<Gender>(
                                          activeColor: AppTheme.accent,
                                          value: variant.value,
                                          groupValue: user.gender,
                                          onChanged: (value) {
                                            context.read<ProfileInfoBloc>().add(
                                              UpdateUserModel(
                                                user: user.copyWith(
                                                  gender: value,
                                                ),
                                              ),
                                            );
                                          },
                                          title: Text(
                                            variant.display,
                                            style: Theme.of(
                                              context,
                                            ).textTheme.bodyMedium?.copyWith(
                                              fontWeight: FontWeight.w500,
                                            ),
                                          ),
                                        );
                                      }).toList(),
                                ),
                              ],
                            ),

                            Row(
                              mainAxisAlignment: MainAxisAlignment.end,
                              children: [
                                Button(
                                  text: "сохранить",
                                  onPressed: () {
                                    context.read<ProfileInfoBloc>().add(
                                      SaveUserInfo(user: user),
                                    );
                                  },
                                ),
                              ],
                            ),

                            const Divider(),

                            Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              spacing: 4,
                              children: [
                                Text(
                                  'Аватар',
                                  style: Theme.of(context).textTheme.bodyLarge,
                                ),
                                Column(
                                  crossAxisAlignment: CrossAxisAlignment.center,
                                  spacing: 16.0,
                                  children: [
                                    Avatar(
                                      width: 80,
                                      name: user.fullName,
                                      url: user.avatar,
                                    ),
                                    Row(
                                      mainAxisAlignment:
                                          MainAxisAlignment.center,
                                      spacing: 4.0,
                                      children: [
                                        Button(
                                          text: "загрузить",
                                          onPressed: () {
                                            context.read<ProfileInfoBloc>().add(
                                              SaveUserAvatar(
                                                avatar: null,
                                              ), // TODO: загрузка фото
                                            );
                                          },
                                        ),
                                        Button(
                                          text: "удалить",
                                          onPressed:
                                              user.avatar == null
                                                  ? null
                                                  : () {
                                                    context
                                                        .read<ProfileInfoBloc>()
                                                        .add(
                                                          SaveUserAvatar(
                                                            avatar: null,
                                                          ),
                                                        );
                                                  },
                                        ),
                                      ],
                                    ),
                                  ],
                                ),
                              ],
                            ),

                            const Divider(),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              spacing: 16,
                              children: [
                                Button(
                                  text: "выйти",
                                  onPressed: () => context.pop(),
                                ),
                                Button(
                                  text: "поддержка",
                                  onPressed: () => context.push("/chat"),
                                ),
                              ],
                            ),
                          ],
                        ),
                      ),
                    ),
                  ],
                ),
                ErrorState(:final errorMessage) => Center(
                  child: Text(errorMessage),
                ),
                _ => const Center(child: Text("Something went wrong.")),
              };
            },
          ),
        ),
      ),
    );
  }
}
