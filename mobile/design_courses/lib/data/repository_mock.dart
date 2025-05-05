import 'course_preview_dto.dart';
import 'course_card_dto.dart';

class MockPreviewRepository {
  static const CourseCardDto sampleCourseOwned = CourseCardDto(
    title: 'Фундаментальные основы Flutter',
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
    title: 'Фундаментальные основы Flutter',
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
    title: 'Фундаментальные основы Flutter',
    category: 'Мобильная разработка',
    description:
        'Изучите Flutter с нуля!\nтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
    imageUrl:
        'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
    price: 2499,
    isOwned: false,
    isArchived: true,
  );

  PreviewDto getSampleCourse() {
    return const PreviewDto(
      title: 'Фундаментальные основы Flutter',
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
    );
  }

  List<CourseCardDto> getAllCourses() {
    return const [
      CourseCardDto(
        title: 'Фундаментальные основы Flutter',
        category: 'Мобильная разработка',
        description:
            'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
        imageUrl:
            'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
        price: 2499,
        isOwned: false,
        isArchived: true,
      ),
      CourseCardDto(
        title: 'Фундаментальные основы Flutter',
        category: 'Мобильная разработка',
        description:
            'Изучите Flutter с нуля!\nтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
        imageUrl:
            'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
        price: 2499,
        isOwned: false,
        isArchived: true,
      ),
      CourseCardDto(
        title: 'Фундаментальные основы Flutter',
        category: 'Мобильная разработка',
        description:
            'Изучите Flutter с нуля!\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
        imageUrl:
            'https://storage.googleapis.com/cms-storage-bucket/f399274b364a6194c43d.png',
        price: 2499,
        isOwned: false,
        isArchived: true,
      ),
    ];
  }
}
