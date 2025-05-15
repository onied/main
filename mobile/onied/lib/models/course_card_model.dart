class CourseCardModel {
  final String id;
  final String title;
  final String category;
  final String description;
  final String imageUrl;
  final int price;
  final bool isOwned;
  final bool isArchived;
  final bool hasCertificates;

  const CourseCardModel({
    required this.id,
    required this.title,
    required this.category,
    required this.description,
    required this.imageUrl,
    required this.price,
    required this.isOwned,
    required this.isArchived,
    required this.hasCertificates,
  });
}
