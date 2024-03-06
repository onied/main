import "./App.css";
import Header from "./components/header/header";
import Course from "./pages/course/course";
import { Route, Router, Routes } from "react-router-dom";

function App() {
  return (
    <>
      <Header></Header>
      <main>
        <Routes>
          <Route path="/course/:courseId/learn/" element={Course()}></Route>
        </Routes>
      </main>
      <main></main>
    </>
  );
}

export default App;
