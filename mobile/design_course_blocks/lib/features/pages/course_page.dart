import 'package:flutter/material.dart';

import '../../app/app_theme.dart';
import '../widgets/block_view.dart';
import '../widgets/course_sidebar.dart';

class CoursePage extends StatefulWidget {
  const CoursePage({super.key});

  @override
  State<CoursePage> createState() => _CoursePageState();
}

class _CoursePageState extends State<CoursePage> {
  int? selectedBlockId;
  bool isSidebarVisible = true;

  final mockHierarchy = {
    'id': 1,
    'modules': [
      {
        'id': 1,
        'title': 'Модуль 1',
        'index': 0,
        'blocks': [
          {'id': 1, 'title': 'Заголовок блока с конспектом', 'blockType': 1, 'index': 0, 'markdownText': 'Рыба текст', 'fileName': 'Название файла', 'fileHref': 'fdsfasd'},
          {'id': 2, 'title': 'Заголовок блока с видео', 'blockType': 2, 'index': 1, 'href': 'https://rutube.ru/video/80185da4f898f4b9ecfb81c1a535b9e1'},
          {'id': 3, 'title': 'Заголовок блока с заданиями', 'blockType': 3, 'index': 2,
            'tasks': [
              {'id': 0, 'title': 'Чипи чипи чапа чапа дуби дуби даба даба?', 'isDone': false, 'taskType': 0, 'maxPoints': 10,
                'variants': [
                  {'id': 0, 'description': 'Чипи чипи'},
                  {'id': 1, 'description': 'Чапа чапа'},
                  {'id': 2, 'description': 'Дуби Дуби'},
                  {'id': 3, 'description': 'Даба Даба'},
                ]
              },
              {'id': 0, 'title': 'Что произошло на пло́щади Тяньаньмэ́нь в 1989 году?', 'isDone': false, 'taskType': 1, 'maxPoints': 10,
                'variants': [
                  {'id': 0, 'description': 'ничего'},
                  {'id': 1, 'description': 'ничего'},
                  {'id': 2, 'description': 'ничего'},
                  {'id': 3, 'description': 'ничего'},
                ]
              },
              {'id': 0, 'title': 'Кто?', 'isDone': false, 'taskType': 2, 'maxPoints': 10,},
              {'id': 0, 'title': 'Напишите эссе на тему: “Как я провел лето”', 'isDone': false, 'taskType': 3, 'maxPoints': 10,}
            ]
          },
        ],
      },
      {
        'id': 1,
        'title': 'Модуль 2',
        'index': 0,
        'blocks': [
          {'id': 4, 'title': 'Заголовок блока с конспектом', 'blockType': 1, 'index': 0},
          {'id': 5, 'title': 'Заголовок блока с виде', 'blockType': 2, 'index': 1, 'href': 'https://vkvideo.ru/video-48265019_456243841'},
          {'id': 6, 'title': 'Заголовок блока с заданиями', 'blockType': 3, 'index': 2},
        ],
      },
    ],
  };

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          "Курс по всякой фигне",
          style: Theme.of(context).textTheme.titleLarge?.copyWith(color: Colors.white),
        ),
        backgroundColor: AppTheme.backgroundColorHeader,
      ),
      drawer: Drawer(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.zero),
        child: CourseSidebar(
          hierarchy: mockHierarchy,
          selectedBlockId: selectedBlockId,
          onBlockSelected: (blockId) {
            setState(() {
              selectedBlockId = blockId;
            });
            Navigator.pop(context);
          },
        ),
      ),
      body: Row(
        children: [
          Expanded(
            child: selectedBlockId != null
              ? BlockView(blockId: selectedBlockId, hierarchy: mockHierarchy)
              : Center(
                child: Text(
                  "Выберите блок",
                  style: Theme.of(context).textTheme.titleLarge
                ),
            ),
          ),
        ],
      ),
    );
  }
}
