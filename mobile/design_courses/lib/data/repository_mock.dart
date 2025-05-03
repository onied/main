import 'course_preview_dto.dart';

class MockPreviewRepository {
  PreviewDto getSampleCourse() {
    return const PreviewDto(
      title: 'Фундаментальные основы Flutter',
      pictureHref:
          'https://lms.redvector.com/lpe/assets/core/img/large-placeholder-course.png',
      description:
          'Изучите Flutter с нуля!\n\nЭтот курс даст вам все необходимые знания для создания кроссплатформенных мобильных приложений.',
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
}
