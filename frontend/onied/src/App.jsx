import "./App.css";
import Header from "./components/header/header";
import Course from "./pages/course/course";
import { Route, Routes } from "react-router-dom";
import Preview from "./pages/preview/preview";
import Catalog from "./pages/catalog/catalog.jsx";
import Register from "./pages/register/register.jsx";
import SignIn from "./pages/signin/signIn";

function App() {
  return (
    <>
      <Header></Header>
      <main>
        <Routes>
          <Route path="/course/:courseId/learn/*" element={<Course />}></Route>
          <Route path="/course/:courseId" element={<Preview />}></Route>
          <Route path="/catalog" element={<Catalog />}></Route>
          <Route path="/register" element={<Register />}></Route>
          <Route path="/signIn" element={<SignIn />}></Route>
        </Routes>
      </main>
    </>
  );
}

export default App;
