import EditTasksBlock from "./editTasksBlock";

type BlockInfo = {
  id: number;
  title: string;
  blockType: number;
};

function EditBlockDispathcer({
  courseId,
  block,
}: {
  courseId: number;
  block: BlockInfo;
}) {
  const blockConstructors = [
    () => <EditTasksBlock courseId={courseId} blockId={Number(block.id)} />,
  ];

  return blockConstructors[block.blockType]();
}

export default EditBlockDispathcer;
