class PreviewDto {
  final String title;
  final String pictureHref;
  final String description;
  final int hoursCount;
  final double price;
  final Category category;
  final CourseAuthor courseAuthor;
  final bool isArchived;
  final bool hasCertificates;
  final List<String>? courseProgram;
  final bool isOwned;

  PreviewDto({
    required this.title,
    required this.pictureHref,
    required this.description,
    required this.hoursCount,
    required this.price,
    required this.category,
    required this.courseAuthor,
    required this.isArchived,
    required this.hasCertificates,
    this.courseProgram,
    required this.isOwned,
  });
}

class Category {
  final int id;
  final String name;

  Category({required this.id, required this.name});
}

class CourseAuthor {
  final String name;
  final String avatarHref;

  CourseAuthor({required this.name, required this.avatarHref});
}

// Example stub/mock data
final mockPreviewDto = PreviewDto(
  title: "Сила",
  pictureHref: "https://example.com/course-image.jpg",
  description: "План как стат силным",
  hoursCount: 12,
  price: 49.99,
  category: Category(id: 1, name: "Физическое развитие"),
  courseAuthor: CourseAuthor(
    name: "Jane Doe",
    avatarHref: "https://example.com/avatars/jane.jpg",
  ),
  isArchived: false,
  hasCertificates: true,
  courseProgram: ["Прес качат", "Турник", "Бегит", "Анжумания"],
  isOwned: false,
);
