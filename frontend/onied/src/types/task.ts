export enum TaskType {
  SingleAnswer = 0,
  MultipleAnswers,
  InputAnswer,
  ManualReview,
};

export type Variant = {
  id: number;
  description: string;
  isNew: boolean;
  isCorrect: boolean;
};

export type Answer = {
  id: number;
  answer: string;
  isNew: boolean;
};

export type Task = {
  id: number;
  title: string;
  taskType: TaskType;
  maxPoints: number;
  isNew: boolean;
  variants?: Variant[];
};

export type SingleAnswerTask = Task & {
  variants: Variant[];
};

export type MultipleAnswersTask = Task & {
  variants: Variant[];
};

export type InputAnswersTask = Task & {
  answers: Answer[];
  isNumber: boolean;
  isCaseSensitive: boolean;
  accuracy: number | null;
};

export type TaskPointsResponse = {
  taskId: number;
  points?: number;
  content: string;
};

export type TaskPointsInfo = {
  checked: boolean;
  points: number;
  maxPoints: number;
};

export type UserInputRequest = {
  taskId: number;
  isDone: boolean;
  variantsIds?: number[];
  answer?: string;
  text?: string;
  taskType: TaskType;
};