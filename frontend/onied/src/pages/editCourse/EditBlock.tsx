import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import EditBlockDispathcer from "../../components/editCourse/editBlocks/editBlockDispatcher";
import api from "../../config/axios";
import { BeatLoader } from "react-spinners";
import { BlockInfo } from "../../types/course";

function EditBlock() {
  const { courseId, blockId } = useParams();
  const [found, setFound] = useState<boolean | undefined>();
  const [block, setBlock] = useState<BlockInfo | null | undefined>(undefined);

  const blockIdNumber = Number.parseInt(blockId!);
  const notFound = <h1 style={{ margin: "3rem" }}>Курс или блок не найден.</h1>;

  useEffect(() => {
    api
      .get("courses/" + courseId + "/hierarchy")
      .then((response) => {
        console.log(response.data);

        console.log(
          response.data.modules
            .map((m: any) => m.blocks)
            .reduce((x: BlockInfo[], y: BlockInfo[]) => x.concat(y))
        );

        const block = response.data.modules
          .map((m: any) => m.blocks)
          .reduce((x: BlockInfo[], y: BlockInfo[]) => x.concat(y))
          .find((b: BlockInfo) => blockIdNumber == b.id);
        setBlock(block);
      })
      .catch((error) => {
        console.log(error);
        if ("response" in error && error.response.status == 404) {
          setFound(false);
        }
      });
  }, [courseId]);

  if (!found && block === null) return notFound;

  if (block === undefined) return <BeatLoader />;

  return (
    <>
      <EditBlockDispathcer
        courseId={Number.parseFloat(courseId!)}
        block={block!}
      />
    </>
  );
}

export default EditBlock;
