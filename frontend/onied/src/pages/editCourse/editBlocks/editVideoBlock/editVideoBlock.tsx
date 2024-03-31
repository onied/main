import classes from "./editVideoBlock.module.css";
import EditVideoBlockComponent from "../../../../components/editCourse/editBlocks/editVideoBlock/editVideoBlock";

function EditVideoBlock() {
  return (
    <>
      <div className={classes.container}>
        <EditVideoBlockComponent />
      </div>
    </>
  );
}

export default EditVideoBlock;
