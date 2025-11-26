import { useNavigate } from "react-router-dom";

export default function Home() {
  const navigate = useNavigate();

  return (
    <div style={{ margin: 20 }}>
      <h1>Bem-vindo!</h1>

      <button onClick={() => navigate("/mentors")}>
        Listar Mentores
      </button>
    </div>
  );
}