import "./App.css";
import Header from "./components/header/header";
import Course from "./pages/course/course";
import { Route, Routes } from "react-router-dom";
import Preview from "./pages/preview/preview";
import Catalog from "./pages/catalog/catalog.jsx";

function App() {
  return (
    <>
      <Header></Header>
      <main>
        <Routes>
          <Route path="/course/:courseId/learn/*" element={<Course />}></Route>
          <Route path="/course/:courseId" element={<Preview />}></Route>
          <Route path="/catalog" element={<Catalog />}></Route>
        </Routes>
      </main>
    </>
  );
}

export default App;
