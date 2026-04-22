import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ThemeProvider } from "./components/theme/ThemeProvider";

import Login from "./pages/Login";
import MentoresPage from "./pages/Mentors";
import Register from "./pages/Register";

export default function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="mentoria-ui-theme">
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/mentores" element={<MentoresPage />} />
          <Route path="/" element={<MentoresPage />} />
        </Routes>
      </BrowserRouter>
    </ThemeProvider>
  );
}
