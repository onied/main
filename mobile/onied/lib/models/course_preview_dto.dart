class PreviewDto {
  final String id;
  final String title;
  final String pictureHref;
  final String description;
  final int hoursCount;
  final int price;
  final Category category;
  final CourseAuthor courseAuthor;
  final bool isArchived;
  final bool hasCertificates;
  final List<String>? courseProgram;
  final bool isOwned;

  const PreviewDto({
    required this.id,
    required this.title,
    required this.pictureHref,
    required this.description,
    required this.hoursCount,
    required this.price,
    required this.category,
    required this.courseAuthor,
    required this.isArchived,
    required this.hasCertificates,
    required this.courseProgram,
    required this.isOwned,
  });
}

class Category {
  final int id;
  final String name;

  const Category({required this.id, required this.name});
}

class CourseAuthor {
  final String name;
  final String avatarHref;

  const CourseAuthor({required this.name, required this.avatarHref});
}
