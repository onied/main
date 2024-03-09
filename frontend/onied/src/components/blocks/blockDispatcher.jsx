import { useParams } from "react-router-dom";

function BlockDispatcher({ hierarchy }) {
  const { blockId } = useParams();
  return <>Здесь будут блоки...</>;
}

export default BlockDispatcher;
