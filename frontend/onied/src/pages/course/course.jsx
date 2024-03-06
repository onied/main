import Sidebar from "../../components/sidebar/sidebar";
import Summary from "../../components/blocks/summary/summary";
import classes from "../../components/blocks/blocks.module.css"

function Course() {
  return (
    <>
      <Sidebar></Sidebar>
      <div className={classes.blockViewContainer}>
        <Summary></Summary>
      </div>
    </>
  );
}
export default Course;
