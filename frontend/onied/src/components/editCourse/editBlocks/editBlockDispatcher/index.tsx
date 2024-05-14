import { BlockType } from "../../../../types/block";
import EditSummaryBlockComponent from "../editSummaryBlock/editSummaryBlock";
import EditTasksBlockComponent from "../editTasksBlock";
import EditVideoBlockComponent from "../editVideoBlock/editVideoBlock";
import classes from "./index.module.css";

type BlockInfo = {
  id: number;
  title: string;
  blockType: BlockType;
};

function EditBlockDispathcer({
  courseId,
  block,
}: {
  courseId: number;
  block: BlockInfo;
}) {
  const blockConstructors = {
    [BlockType.AnyBlock]: () => null,
    [BlockType.SummaryBlock]: () => (
      <EditSummaryBlockComponent
        courseId={courseId}
        blockId={Number(block.id)}
      />
    ),
    [BlockType.VideoBlock]: () => (
      <EditVideoBlockComponent courseId={courseId} blockId={Number(block.id)} />
    ),
    [BlockType.TasksBlock]: () => (
      <EditTasksBlockComponent courseId={courseId} blockId={Number(block.id)} />
    ),
  };

  return (
    <div className={classes.container}>
      {blockConstructors[block.blockType]()}
    </div>
  );
}

export default EditBlockDispathcer;
