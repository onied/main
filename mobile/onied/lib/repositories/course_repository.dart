import 'package:collection/collection.dart';
import 'package:onied_mobile/models/course_block_model.dart';
import 'package:onied_mobile/models/course_hierarchy_model.dart';
import 'package:onied_mobile/models/course_preview_model.dart';
import 'package:onied_mobile/models/course_card_model.dart';
import 'package:onied_mobile/models/enums/block_type.dart';
import 'package:onied_mobile/models/enums/task_type.dart';

typedef CoursesFilterPredicate = bool Function(CourseCardModel courseCardDto);

class CourseRepository {
  static const allCategories = [
    'Мобильная разработка',
    'Категория 2',
    'Категория 3',
  ];

  static final CourseCardModel sampleCourseOwned = CourseCardModel(
    id: "1",
    title: 'Фундаментальные основы Flutter один',
    category: allCategories[0],
    description:
        'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
    imageUrl:
        'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
    price: 2499,
    isOwned: true,
    isArchived: false,
    hasCertificates: false,
  );

  static final CourseCardModel sampleCourseNotOwnedOrArchived = CourseCardModel(
    id: "2",
    title: 'Фундаментальные основы Flutter два',
    category: allCategories[1],
    description:
        'Изучите Flutter с нуля!\nтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
    imageUrl:
        'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
    price: 2499,
    isOwned: false,
    isArchived: false,
    hasCertificates: true,
  );

  static final CourseCardModel sampleCourseArchived = CourseCardModel(
    id: "3",
    title: 'Фундаментальные основы Flutter три',
    category: allCategories[2],
    description:
        'Изучите Flutter с нуля!\nтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
    imageUrl:
        'https://lms.redvector.com/lpe/assets/core/img/large-placeholder-course.png',
    price: 2499,
    isOwned: false,
    isArchived: true,
    hasCertificates: true,
  );

  static List<CoursePreviewModel> samplePreviewDtos = [
    CoursePreviewModel(
      id: "1",
      title: 'Фундаментальные основы Flutter один',
      pictureHref:
          'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
      description:
          'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
      hoursCount: 12,
      price: 2499,
      category: Category(id: 1, name: allCategories[0]),
      courseAuthor: const CourseAuthor(
        name: 'Иван Иванов',
        avatarHref: 'https://www.w3schools.com/howto/img_avatar.png',
      ),
      isArchived: false,
      hasCertificates: false,
      courseProgram: const [
        'Введение в Flutter и Dart',
        'Работа с виджетами',
        'Состояние и управление состоянием',
        'Работа с сетевыми запросами',
        'Публикация приложения',
      ],
      isOwned: true,
    ),
    CoursePreviewModel(
      id: "2",
      title: 'Фундаментальные основы Flutter два',
      pictureHref:
          'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
      description:
          'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
      hoursCount: 12,
      price: 2499,
      category: Category(id: 2, name: allCategories[1]),
      courseAuthor: const CourseAuthor(
        name: 'Иван Иванов',
        avatarHref: 'https://www.w3schools.com/howto/img_avatar.png',
      ),
      isArchived: false,
      hasCertificates: true,
      courseProgram: const [
        'Введение в Flutter и Dart',
        'Работа с виджетами',
        'Состояние и управление состоянием',
        'Работа с сетевыми запросами',
        'Публикация приложения',
      ],
      isOwned: false,
    ),
    CoursePreviewModel(
      id: "3",
      title: 'Фундаментальные основы Flutter три',
      pictureHref:
          'https://lms.redvector.com/lpe/assets/core/img/large-placeholder-course.png',
      description:
          'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
      hoursCount: 12,
      price: 2499,
      category: Category(id: 3, name: allCategories[2]),
      courseAuthor: const CourseAuthor(
        name: 'Иван Иванов',
        avatarHref: 'https://www.w3schools.com/howto/img_avatar.png',
      ),
      isArchived: true,
      hasCertificates: true,
      courseProgram: const [
        'Введение в Flutter и Dart',
        'Работа с виджетами',
        'Состояние и управление состоянием',
        'Работа с сетевыми запросами',
        'Публикация приложения',
      ],
      isOwned: false,
    ),
  ];

  static CourseHierarchyModel sampleCourseHierarchy = CourseHierarchyModel(
    id: 1,
    title: "Курс по какой-то фигне",
    modules: [
      CourseHierarchyModule(
        id: 1,
        title: 'Модуль 1',
        index: 0,
        blocks: [
          CourseHierarchyBlock(
            id: 1,
            title: 'Заголовок блока с конспектом',
            blockType: BlockType.summaryBlock,
            index: 0,
            completed: false,
          ),
          CourseHierarchyBlock(
            id: 2,
            title: 'Заголовок блока с видео',
            blockType: BlockType.videoBlock,
            index: 1,
            completed: false,
          ),
          CourseHierarchyBlock(
            id: 3,
            title: 'Заголовок блока с заданиями',
            blockType: BlockType.tasksBlock,
            index: 2,
            completed: false,
          ),
        ],
      ),
    ],
  );

  static List<CourseBlockModel> sampleBlocks = [
    CourseConspectBlockModel(
      id: 1,
      title: 'Заголовок блока с конспектом',
      index: 0,
      markdownText: "Рыба **файл**.",
      fileName: "Название файла",
      fileHref: "fdsfasd",
    ),
    CourseVideoBlockModel(
      id: 2,
      title: 'Заголовок блока с конспектом',
      index: 1,
      href: 'https://rutube.ru/video/80185da4f898f4b9ecfb81c1a535b9e1',
    ),
    CourseTaskBlockModel(
      id: 3,
      title: 'Заголовок блока с конспектом',
      index: 2,
      tasks: [
        CourseTaskBlockTask(
          id: 0,
          title: 'Чипи чипи чапа чапа дуби дуби даба даба?',
          isDone: false,
          taskType: TaskType.singleAnswer,
          maxPoints: 10,
          variants: [
            CourseTaskBlockTaskVariant(id: 0, description: 'Чипи чипи'),
            CourseTaskBlockTaskVariant(id: 2, description: 'Чапа чапа'),
            CourseTaskBlockTaskVariant(id: 3, description: 'Дуби Дуби'),
            CourseTaskBlockTaskVariant(id: 4, description: 'Даба Даба'),
          ],
        ),
        CourseTaskBlockTask(
          id: 1,
          title: 'Что произошло на пло́щади Тяньаньмэ́нь в 1989 году?',
          isDone: false,
          taskType: TaskType.multipleAnswers,
          maxPoints: 10,
          variants: [
            CourseTaskBlockTaskVariant(id: 0, description: 'ничего'),
            CourseTaskBlockTaskVariant(id: 2, description: 'ничего'),
            CourseTaskBlockTaskVariant(id: 3, description: 'ничего'),
            CourseTaskBlockTaskVariant(id: 4, description: 'ничего'),
          ],
        ),
        CourseTaskBlockTask(
          id: 2,
          title: 'Кто?',
          isDone: false,
          taskType: TaskType.inputAnswer,
          maxPoints: 10,
        ),
        CourseTaskBlockTask(
          id: 3,
          title: 'Напишите эссе на тему: “Как я провел лето”',
          isDone: false,
          taskType: TaskType.manualReview,
          maxPoints: 10,
        ),
      ],
    ),
  ];

  Future<Iterable<String>> getAllCategories() async {
    return allCategories;
  }

  Future<CoursePreviewModel?> getCourseById(String courseId) async {
    return samplePreviewDtos
        .where((previewDto) => previewDto.id == courseId)
        .single;
  }

  Future<Iterable<CourseCardModel>> getAllCourses() async {
    return [
      ...List.generate(30, (_) => sampleCourseOwned),
      ...List.generate(30, (_) => sampleCourseNotOwnedOrArchived),
      ...List.generate(30, (_) => sampleCourseArchived),
    ];
  }

  Future<Iterable<CourseCardModel>> getFilteredCourses(
    String searchQuery,
    CoursesFilterPredicate filterPredicate,
  ) async {
    return (await getAllCourses())
        .where((course) => course.title.contains(searchQuery))
        .where(filterPredicate)
        .toList();
  }

  Future<Iterable<CourseCardModel>> getOwnedCourses() async {
    return List.generate(30, (_) => sampleCourseOwned);
  }

  Future<Iterable<CourseCardModel>> getPopularCourses() async {
    return List.generate(30, (_) => sampleCourseNotOwnedOrArchived);
  }

  Future<Iterable<CourseCardModel>> getRecommendedCourses() async {
    return List.generate(30, (_) => sampleCourseNotOwnedOrArchived);
  }

  Future<CourseHierarchyModel?> getCourseHierarchy(int courseId) async {
    return sampleCourseHierarchy;
  }

  Future<CourseBlockModel?> getCourseBlockById(int blockId) async {
    return sampleBlocks.firstWhereOrNull((block) => block.id == blockId);
  }
}
