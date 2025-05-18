abstract class CoursePreviewBlocEvent {}

class LoadCoursePreview extends CoursePreviewBlocEvent {
  final String courseId;
  LoadCoursePreview(this.courseId);
}
