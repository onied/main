class CourseCardModel {
  final int id;
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

  factory CourseCardModel.fromJson(Map<String, dynamic> json) {
    return CourseCardModel(
      id: json["id"] as int,
      title: json["title"] as String,
      category:
          (json['category'] as Map<String, dynamic>?)?['name'] as String? ?? '',
      description: json["description"] as String,
      imageUrl: json["pictureHref"] as String,
      price: json["priceRubles"] as int,
      isOwned: json["isOwned"] as bool,
      isArchived: json["isArchived"] as bool,
      hasCertificates: json["hasCertificates"] as bool,
    );
  }
}
