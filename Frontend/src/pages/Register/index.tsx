import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { supabase } from "@/auth/supabase";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { PasswordInput } from "@/components/ui/password-input";

export default function Register() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const register = async (e?: React.FormEvent) => {
    if (e) e.preventDefault();
    setError("");

    if (password !== confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    setLoading(true);

    const { error } = await supabase.auth.signUp({
      email,
      password,
    });

    setLoading(false);

    if (error) {
      setError(error.message);
      return;
    }

    navigate("/login");
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-[linear-gradient(to_right,var(--color-brand-100)_0%,var(--color-brand-100)_50%,var(--color-brand-200)_50%,var(--color-brand-200)_100%)] p-6">
      <div className="min-h-screen md:min-h-[70vh] w-full max-w-5xl flex flex-col md:flex-row bg-white rounded-3xl shadow-2xl overflow-hidden">
        <div className="p-8 md:w-[49%]">
          <button
            type="button"
            onClick={() => navigate("/mentors")}
            aria-label="Voltar"
            title="Voltar"
            className="mb-4 inline-flex h-9 w-9 items-center justify-center rounded-full border border-brand-300 bg-white text-brand-700 shadow-sm hover:bg-brand-50 focus:outline-none focus:ring-2 focus:ring-brand-500"
          >
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" className="text-brand-700">
              <path d="M15 6l-6 6 6 6" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
            </svg>
          </button>

          <form onSubmit={register} className="w-full">
            <div className="mb-5">
              <h1 className="text-2xl font-semibold text-gray-900">Bem-vindo!</h1>
              <p className="mt-2 text-gray-600">Estamos felizes por você estar aqui. Sua escolha de ensinar ou aprender já demonstra coragem para crescer.</p>
            </div>


            {error && (
              <Alert variant="destructive" className="mb-4 border-red-200 bg-red-50 text-red-700">
                <AlertDescription>{error}</AlertDescription>
              </Alert>
            )}

            <div className="mb-4">
              <label className="block text-sm text-gray-700" htmlFor="email">Email</label>
              <input
                id="email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="you@example.com"
                autoComplete="email"
                required
                className="mt-1 w-full rounded-xl border border-gray-200 px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-brand-500"
              />
            </div>

            <div className="mb-4">
              <PasswordInput
                label="Senha"
                id="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
                autoComplete="new-password"
                required
                aria-invalid={!!error}
              />
            </div>

            <div className="mb-6">
              <PasswordInput
                label="Confirmar senha"
                id="confirm"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                placeholder="••••••••"
                autoComplete="new-password"
                required
              />
            </div>

            <button
              type="submit"
              disabled={loading || !email || !password || !confirmPassword}
              className="w-full rounded-xl bg-brand-600 px-3 py-2 text-white transition hover:bg-brand-500 disabled:cursor-not-allowed disabled:opacity-80"
            >
              {loading ? "Criando..." : "Criar conta"}
            </button>

            <div className="mt-4 text-center text-sm">
              <span className="text-gray-600">Já tem conta?</span>{" "}
              <button type="button" onClick={() => navigate("/login")} className="text-brand-600 hover:text-brand-700 cursor-pointer">
                Entrar
              </button>
            </div>
          </form>
        </div>

        <div className="relative flex items-center justify-center bg-brand-50 md:flex-1 md:min-h-[80vh] overflow-visible">
          <img
            src="/img/login-mentoria.png"
            alt="Ilustração de mentoria"
            className="w-[130%] max-w-none object-cover -translate-x-3 pointer-events-none"
            loading="lazy"
            onError={(e) => {
              e.currentTarget.src = "/vite.svg";
            }}
          />
        </div>
      </div>
    </div>
  );
}
