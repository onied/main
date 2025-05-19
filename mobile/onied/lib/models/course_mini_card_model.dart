class CourseMiniCardModel {
  final int id;
  final String title;
  final String imageUrl;

  const CourseMiniCardModel({
    required this.id,
    required this.title,
    required this.imageUrl,
  });

  factory CourseMiniCardModel.fromJson(Map<String, dynamic> json) {
    return CourseMiniCardModel(
      id: json["id"] as int,
      title: json["title"] as String,
      imageUrl: json["pictureHref"] as String,
    );
  }
}
