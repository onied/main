// import { useEffect, useState } from "react";
// import { useParams } from "react-router-dom";
// import EditBlockDispathcer from "../../components/editCourse/editBlocks/editBlockDispatcher";
// import api from "../../../config/axios";

// type BlockInfo = {
//   id: number;
//   title: string;
//   blockType: number;
// };

// type Module = {
//   id: number;
//   title: string;
//   blocks: Array<BlockInfo>;
// };

// type Course = {
//   id: number;
//   modules: Array<Module>;
//   title: string;
// };

// function EditBlockPage() {
//   const { courseId, blockId } = useParams();
//   const { found, setFound } = useState<boolean | undefined>();
//   const { block, setBlock } = useState<BlockInfo | null | undefined>(undefined);

//   useEffect(() => {
//     api
//       .get("courses/" + courseId + "/hierarchy")
//       .then((response) => {
//         console.log(response.data);
//         setBlock(response.data);
//       })
//       .catch((error) => {
//         console.log(error);
//         if ("response" in error && error.response.status == 404) {
//           setFound(false);
//         }
//       });
//   }, [courseId]);

//   return <>
//     <EditBlockDispathcer courseId={Number.parseFloat(courseId!)} blockId={blockId} />
//   </>;
// }

// export default EditBlockPage;
