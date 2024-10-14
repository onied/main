export type Course = {
  id: number;
  title: string;
  modules: Array<Module>;
};

export type Module = {
  id: number;
  index: number;
  title: string;
  blocks: Array<BlockInfo>;
};

export type BlockInfo = {
  id: number;
  title: string;
  blockType: number;
};
