class CourseCardDto {
  final int id;
  final String title;
  final String category;
  final String description;
  final String imageUrl;
  final int price;
  final bool isOwned;
  final bool isArchived;

  const CourseCardDto({
    required this.id,
    required this.title,
    required this.category,
    required this.description,
    required this.imageUrl,
    required this.price,
    required this.isOwned,
    required this.isArchived,
  });
}
