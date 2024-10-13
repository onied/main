import { Task } from "./task";

export enum BlockType {
  AnyBlock = 0,
  SummaryBlock,
  VideoBlock,
  TasksBlock,
}

export type Block = {
  id: number;
  index: number;
  title: string;
  blockType: BlockType;
  completed: boolean;
};

export type TasksBlock = Block & {
  tasks: Task[];
};

export type VideoBlock = Block & {
  href: string;
};
