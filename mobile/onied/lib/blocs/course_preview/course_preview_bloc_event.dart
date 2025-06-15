abstract class CoursePreviewBlocEvent {}

class LoadCoursePreview extends CoursePreviewBlocEvent {
  final String courseId;
  LoadCoursePreview(this.courseId);
}

class LikeCurrentCourse extends CoursePreviewBlocEvent {
  final bool like;
  LikeCurrentCourse(this.like);
}

class OpenStats extends CoursePreviewBlocEvent {}

class CloseStats extends CoursePreviewBlocEvent {}

class UpdateStats extends CoursePreviewBlocEvent {
  final int likes;
  UpdateStats(this.likes);
}
