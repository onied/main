class CoursePreviewModel {
  final int id;
  final String title;
  final String pictureHref;
  final String description;
  final int hoursCount;
  final int price;
  final CategoryModel category;
  final CourseAuthorModel courseAuthor;
  final bool isArchived;
  final bool hasCertificates;
  final List<String>? courseProgram;
  final bool isOwned;
  final bool isLiked;

  const CoursePreviewModel({
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
    required this.isLiked,
  });

  factory CoursePreviewModel.fromJson(Map<String, dynamic> json) {
    return CoursePreviewModel(
      id: json['id'] as int,
      title: json['title'] as String,
      pictureHref: json['pictureHref'] as String,
      description: json['description'] as String,
      hoursCount: json['hoursCount'] as int,
      price: json['priceRubles'] as int,
      isArchived: json['isArchived'] as bool,
      hasCertificates: json['hasCertificates'] as bool,
      isOwned: json['isOwned'] as bool,
      isLiked: json['isLiked'] as bool,
      category: CategoryModel(
        id: (json['category'] as Map<String, dynamic>?)?['id'] as int,
        name: (json['category'] as Map<String, dynamic>?)?['name'] as String,
      ),
      courseAuthor: CourseAuthorModel(
        name:
            ((json['author'] as Map<String, dynamic>?)?['firstName']
                as String) +
            ((json['author'] as Map<String, dynamic>?)?['lastName'] as String),
        avatarHref:
            (json['author'] as Map<String, dynamic>?)?['avatarHref'] as String,
      ),
      courseProgram:
          (json['modules'] as List<dynamic>?)
              ?.map(
                (m) => (m as Map<String, dynamic>)['title'] as String? ?? '',
              )
              .toList() ??
          <String>[],
    );
  }

  CoursePreviewModel copyWith({
    int? id,
    String? title,
    String? pictureHref,
    String? description,
    int? hoursCount,
    int? price,
    CategoryModel? category,
    CourseAuthorModel? courseAuthor,
    bool? isArchived,
    bool? hasCertificates,
    List<String>? courseProgram,
    bool? isOwned,
    bool? isLiked,
  }) {
    return CoursePreviewModel(
      id: id ?? this.id,
      title: title ?? this.title,
      pictureHref: pictureHref ?? this.pictureHref,
      description: description ?? this.description,
      hoursCount: hoursCount ?? this.hoursCount,
      price: price ?? this.price,
      category: category ?? this.category,
      courseAuthor: courseAuthor ?? this.courseAuthor,
      isArchived: isArchived ?? this.isArchived,
      hasCertificates: hasCertificates ?? this.hasCertificates,
      courseProgram: courseProgram ?? this.courseProgram,
      isOwned: isOwned ?? this.isOwned,
      isLiked: isLiked ?? this.isLiked,
    );
  }
}

class CategoryModel {
  final int id;
  final String name;

  const CategoryModel({required this.id, required this.name});

  factory CategoryModel.fromJson(Map<String, dynamic> json) {
    return CategoryModel(id: json["id"] as int, name: json["name"]);
  }

  @override
  bool operator ==(Object other) =>
      identical(this, other) ||
      other is CategoryModel &&
          runtimeType == other.runtimeType &&
          id == other.id &&
          name == other.name;

  @override
  int get hashCode => id.hashCode ^ name.hashCode;
}

class CourseAuthorModel {
  final String name;
  final String avatarHref;

  const CourseAuthorModel({required this.name, required this.avatarHref});
}
