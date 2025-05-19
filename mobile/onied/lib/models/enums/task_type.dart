enum TaskType { singleAnswer, multipleAnswers, inputAnswer, manualReview }

TaskType taskTypeFromString(String value) {
  switch (value.toUpperCase()) {
    case "SINGLE_ANSWER":
      return TaskType.singleAnswer;
    case "MULTIPLE_ANSWERS":
      return TaskType.multipleAnswers;
    case "INPUT_ANSWER":
      return TaskType.inputAnswer;
    case "MANUAL_REVIEW":
      return TaskType.manualReview;
    default:
      throw UnimplementedError("Can't return TaskType for given string");
  }
}
