export enum TaskType {
  SingleAnswer = 0,
  MultipleAnswers,
  InputAnswer,
  ManualReview,
}

export type Answer = {
  id: number;
  description: string;
};

export type Task = {
  id: number;
  title: string;
  taskType: TaskType;
  maxPoints: number;
  variants: Answer[] | null;
};
