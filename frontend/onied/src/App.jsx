import "./App.css";
import Header from "./components/header/header";
import Course from "./pages/course/course";
import CourseEmbedVideo from "./pages/course/embedVideoExample";
import { Route, Router, Routes } from "react-router-dom";

function App() {
  return (
    <>
      <Header></Header>
      <main>
        <Routes>
          <Route path="/course/:courseId/learn/" element={Course()}></Route>
          <Route path="/course/:courseId/learn/videoExample" element={CourseEmbedVideo()}></Route>
        </Routes>
      </main>
    </>
  );
}

export default App;