import 'course_preview_dto.dart';
import 'course_card_dto.dart';

class MockPreviewRepository {
  static const CourseCardDto sampleCourseOwned = CourseCardDto(
    id: 1,
    title: 'Фундаментальные основы Flutter один',
    category: 'Мобильная разработка',
    description:
        'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
    imageUrl:
        'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
    price: 2499,
    isOwned: true,
    isArchived: false,
  );

  static const CourseCardDto sampleCourseNotOwnedOrArchived = CourseCardDto(
    id: 2,
    title: 'Фундаментальные основы Flutter два',
    category: 'Мобильная разработка',
    description:
        'Изучите Flutter с нуля!\nтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
    imageUrl:
        'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
    price: 2499,
    isOwned: false,
    isArchived: false,
  );

  static const CourseCardDto sampleCourseArchived = CourseCardDto(
    id: 3,
    title: 'Фундаментальные основы Flutter три',
    category: 'Мобильная разработка',
    description:
        'Изучите Flutter с нуля!\nтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
    imageUrl:
        'https://lms.redvector.com/lpe/assets/core/img/large-placeholder-course.png',
    price: 2499,
    isOwned: false,
    isArchived: true,
  );

  static const List<PreviewDto> samplePreviewDtos = [
    PreviewDto(
      id: 1,
      title: 'Фундаментальные основы Flutter один',
      pictureHref:
          'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
      description:
          'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
      hoursCount: 12,
      price: 2499,
      category: Category(id: 1, name: 'Мобильная разработка'),
      courseAuthor: CourseAuthor(
        name: 'Иван Иванов',
        avatarHref: 'https://www.w3schools.com/howto/img_avatar.png',
      ),
      isArchived: false,
      hasCertificates: true,
      courseProgram: [
        'Введение в Flutter и Dart',
        'Работа с виджетами',
        'Состояние и управление состоянием',
        'Работа с сетевыми запросами',
        'Публикация приложения',
      ],
      isOwned: true,
    ),
    PreviewDto(
      id: 2,
      title: 'Фундаментальные основы Flutter два',
      pictureHref:
          'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
      description:
          'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
      hoursCount: 12,
      price: 2499,
      category: Category(id: 1, name: 'Мобильная разработка'),
      courseAuthor: CourseAuthor(
        name: 'Иван Иванов',
        avatarHref: 'https://www.w3schools.com/howto/img_avatar.png',
      ),
      isArchived: false,
      hasCertificates: true,
      courseProgram: [
        'Введение в Flutter и Dart',
        'Работа с виджетами',
        'Состояние и управление состоянием',
        'Работа с сетевыми запросами',
        'Публикация приложения',
      ],
      isOwned: false,
    ),
    PreviewDto(
      id: 3,
      title: 'Фундаментальные основы Flutter три',
      pictureHref:
          'https://lms.redvector.com/lpe/assets/core/img/large-placeholder-course.png',
      description:
          'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
      hoursCount: 12,
      price: 2499,
      category: Category(id: 1, name: 'Мобильная разработка'),
      courseAuthor: CourseAuthor(
        name: 'Иван Иванов',
        avatarHref: 'https://www.w3schools.com/howto/img_avatar.png',
      ),
      isArchived: true,
      hasCertificates: true,
      courseProgram: [
        'Введение в Flutter и Dart',
        'Работа с виджетами',
        'Состояние и управление состоянием',
        'Работа с сетевыми запросами',
        'Публикация приложения',
      ],
      isOwned: false,
    ),
  ];

  PreviewDto getCourseById(int courseId) {
    return samplePreviewDtos
        .where((previewDto) => previewDto.id == courseId)
        .single;
  }

  List<CourseCardDto> getAllCourses() {
    return [
      ...List.generate(30, (_) => sampleCourseOwned),
      ...List.generate(30, (_) => sampleCourseNotOwnedOrArchived),
      ...List.generate(30, (_) => sampleCourseArchived),
    ];
  }

  List<CourseCardDto> getFilteredCourses(String searchQuery) {
    return getAllCourses()
        .where((course) => course.title.contains(searchQuery))
        .toList();
  }

  List<CourseCardDto> getOwnedCourses() {
    return List.generate(30, (_) => sampleCourseOwned);
  }

  List<CourseCardDto> getPopularCourses() {
    return List.generate(30, (_) => sampleCourseNotOwnedOrArchived);
  }

  List<CourseCardDto> getRecommendedCourses() {
    return List.generate(30, (_) => sampleCourseNotOwnedOrArchived);
  }
}
