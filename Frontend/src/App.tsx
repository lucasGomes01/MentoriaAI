import { BrowserRouter, Routes, Route } from "react-router-dom";

import Login from "./pages/Login";
import MentoresPage from "./pages/Mentors";
import Register from "./pages/Register";

import ProtectedRoute from "./components/layout/ProtectedRoute";

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/mentores" element={<MentoresPage />} />
        <Route path="/" element={<MentoresPage />} />
      </Routes>
    </BrowserRouter>
  );
}
