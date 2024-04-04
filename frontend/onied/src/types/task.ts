export enum TaskType {
  SingleAnswer = 0,
  MultipleAnswers,
  InputAnswer,
  ManualReview,
}

export type Variant = {
  id: number;
  description: string;
};

export type Task = {
  id: number;
  title: string;
  taskType: TaskType;
  maxPoints: number;
};

export type SingleAnswerTask = Task & {
  variants: Variant[] | null;
  rightVariant: number;
};

export type MultipleAnswersTask = Task & {
  variants: Variant[] | null;
  rightVariants: number[] | null;
};
