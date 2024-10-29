import { useParams } from "react-router-dom";
import Summary from "./summary/summary";
import Tasks from "./tasks/tasks";
import Video from "./video/video";
import { useEffect } from "react";
import classes from "./blocks.module.css";
import NotFound from "../general/responses/notFound/notFound";
import CustomBeatLoader from "../general/customBeatLoader";
import { Course } from "@onied/types/course";
import { Block } from "@onied/types/block";

type props = {
  hierarchy: Course;
  setCurrentBlock: (blockId: number) => void;
};

function BlockDispatcher({ hierarchy, setCurrentBlock }: props) {
  const { blockId } = useParams();

  const blockIdNumber = Number(blockId);
  const blockTypes = [
    () => <></>,
    () => <Summary courseId={hierarchy.id} blockId={blockIdNumber} />,
    () => <Video courseId={hierarchy.id} blockId={blockIdNumber} />,
    () => <Tasks courseId={hierarchy.id} blockId={blockIdNumber} />,
  ];

  useEffect(() => setCurrentBlock(blockIdNumber));

  const blocks: { [blockId: number]: Block } | undefined =
    hierarchy != null && "modules" in hierarchy
      ? hierarchy.modules
          .flatMap((module) => module.blocks)
          .reduce((acc, cur) => ({ ...acc, [cur.id]: cur }), {})
      : undefined;

  if (
    isNaN(blockIdNumber) ||
    blocks === undefined ||
    !Object.keys(blocks).includes(blockId!)
  )
    return <NotFound>Блок не найден.</NotFound>;

  return (
    <div className={classes.blockWrapper}>
      {blocks ? (
        blockTypes[blocks[blockIdNumber].blockType]()
      ) : (
        <CustomBeatLoader />
      )}
    </div>
  );
}

export default BlockDispatcher;
