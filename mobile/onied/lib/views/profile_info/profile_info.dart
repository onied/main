import 'package:flutter/material.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/components/avatar/avatar.dart';
import 'package:onied_mobile/components/button/button.dart';

class ProfileInfoPage extends StatelessWidget {
  const ProfileInfoPage({super.key});

  @override
  Widget build(BuildContext context) {
    const model = (
      firstName: "Admin",
      lastName: "Admin",
      gender: 0,
      avatar: "",
      email: "admin@admin.admin",
    );

    final fullName = '${model.firstName} ${model.lastName}';

    return Scaffold(
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
        child: Column(
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
                  Avatar(width: 80, name: fullName),
                  Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    mainAxisAlignment: MainAxisAlignment.center,
                    spacing: 4.0,
                    children: [
                      Text(
                        fullName,
                        style: TextStyle(
                          fontSize: 20,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      Text(
                        model.email,
                        style: TextStyle(fontSize: 16, color: Colors.grey),
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
                          initialValue: model.firstName,
                          decoration: InputDecoration(
                            hintText: "Имя",
                            hintStyle: Theme.of(context).textTheme.bodyMedium
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
                          initialValue: model.lastName,
                          decoration: InputDecoration(
                            hintText: 'Фамилия',
                            hintStyle: Theme.of(context).textTheme.bodyMedium
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
                                (value: 0, display: "Не указан"),
                                (value: 1, display: "Мужской"),
                                (value: 2, display: "Женский"),
                              ].map<Widget>((variant) {
                                return RadioListTile<int>(
                                  activeColor: AppTheme.accent,
                                  value: variant.value,
                                  groupValue: 0,
                                  onChanged: (value) {},
                                  title: Text(
                                    variant.display,
                                    style: Theme.of(context)
                                        .textTheme
                                        .bodyMedium
                                        ?.copyWith(fontWeight: FontWeight.w500),
                                  ),
                                );
                              }).toList(),
                        ),
                      ],
                    ),

                    Row(
                      mainAxisAlignment: MainAxisAlignment.end,
                      children: [
                        Button(text: "сохранить", onPressed: () {}),
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
                            Avatar(width: 80, name: "Admin Admin"),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.center,
                              spacing: 4.0,
                              children: [
                                Button(
                                  text: "загрузить",
                                  onPressed: () {},
                                ),
                                Button(text: "удалить", onPressed: null),
                              ],
                            ),
                          ],
                        ),
                      ],
                    ),

                    const Divider(),

                    Button(text: "выйти", onPressed: () {}),
                  ],
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
