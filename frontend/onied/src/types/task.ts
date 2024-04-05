export enum TaskType {
  SingleAnswer = 0,
  MultipleAnswers,
  InputAnswer,
  ManualReview,
}
``;

export type Variant = {
  id: number;
  description: string;
  isNew: boolean;
};

export type Answer = {
  id: number;
  description: string;
  isNew: boolean;
};

export type Task = {
  id: number;
  title: string;
  taskType: TaskType;
  maxPoints: number;
  isNew: boolean;
};

export type SingleAnswerTask = Task & {
  variants: Variant[];
  rightVariant: number;
};

export type MultipleAnswersTask = Task & {
  variants: Variant[];
  rightVariants: number[];
};

export type InputAnswersTask = Task & {
  answers: Answer[];
  isNumber: boolean;
  checkRegister: boolean;
  checkAccuracy: number | null;
};
