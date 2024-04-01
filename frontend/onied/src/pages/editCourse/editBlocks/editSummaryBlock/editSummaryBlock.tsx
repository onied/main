import EditSummaryBlockComponent from "../../../../components/editCourse/editBlocks/editSummaryBlock/editSummaryBlock";
import classes from "./editSummaryBlock.module.css";

function EditSummaryBlock() {
  return (
    <>
      <div className={classes.summaryEditWrapper}>
        <EditSummaryBlockComponent />
      </div>
    </>
  );
}

export default EditSummaryBlock;
