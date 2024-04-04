export type Course = {
  id: number;
  modules: Array<Module>;
  title: string;
};

export type Module = {
  id: number;
  title: string;
  blocks: Array<BlockInfo>;
};

export type BlockInfo = {
  id: number;
  title: string;
  blockType: number;
};
