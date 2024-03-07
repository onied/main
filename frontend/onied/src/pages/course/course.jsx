import Sidebar from "../../components/sidebar/sidebar";
import Summary from "../../components/blocks/summary/summary";
import classes from "../../components/blocks/blocks.module.css";
import Tasks from "../../components/blocks/tasks/tasks";
import { Route, Routes } from "react-router-dom";

function Course() {
  return (
    <>
      <Sidebar></Sidebar>
      <div className={classes.blockViewContainer}>
        <Routes>
          <Route path="1" element={Summary()}></Route>
          <Route path="2" element={Tasks()}></Route>
        </Routes>
      </div>
    </>
  ); // /course/asdf/learn/1 -- summary, /course/asdf/learn/2 -- tasks
}
export default Course;
