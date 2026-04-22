import { useNavigate } from "react-router-dom";
import { supabase } from "../../auth/supabase";
import { useState } from "react";
import { PasswordInput } from "@/components/ui/password-input";

export default function Login() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const login = async (e?: React.FormEvent) => {
    if (e) e.preventDefault();
    setError("");
    setLoading(true);

    const { error, data } = await supabase.auth.signInWithPassword({
      email,
      password,
    });

    setLoading(false);

    if (error) {
      setError(error.message);
      return;
    }

    if (data.session?.access_token) {
      localStorage.setItem("access_token", data.session.access_token);
    }

    navigate("/mentores");
  };

return (
  <div className="min-h-screen flex items-center justify-center bg-[linear-gradient(to_right,var(--color-brand-100)_0%,var(--color-brand-100)_50%,var(--color-brand-200)_50%,var(--color-brand-200)_100%)] dark:bg-[linear-gradient(to_right,var(--color-slate-900)_0%,var(--color-slate-900)_50%,var(--color-slate-950)_50%,var(--color-slate-950)_100%)] p-6 transition-colors duration-300">
    <div className="min-h-screen md:min-h-[70vh] w-full max-w-5xl flex flex-col md:flex-row bg-white dark:bg-slate-900 rounded-3xl shadow-2xl overflow-visible border border-transparent dark:border-slate-800">
      
      <div className="p-8 md:w-[49%]">
        <button
          type="button"
          onClick={() => navigate("/mentors")}
          aria-label="Voltar"
          title="Voltar"
          className="mb-4 inline-flex h-9 w-9 items-center justify-center rounded-full border border-brand-300 dark:border-slate-700 bg-white dark:bg-slate-800 text-brand-700 dark:text-gray-300 shadow-sm hover:bg-brand-50 dark:hover:bg-slate-700 focus:outline-none focus:ring-2 focus:ring-brand-500"
        >
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" className="text-brand-700">
            <path d="M15 6l-6 6 6 6" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
          </svg>
        </button>

        <form onSubmit={login} className="w-full">
          <div className="mb-5">
            <h1 className="text-2xl font-semibold text-gray-900 dark:text-white">Bem-vindo de volta</h1>
            <p className="mt-2 text-gray-600 dark:text-gray-400">Acesse sua conta para continuar.</p>
          </div>

          {error && (
            <div role="alert" className="mb-4 rounded-xl border border-red-200 dark:border-red-900 bg-red-50 dark:bg-red-950/50 px-3 py-2 text-sm text-red-700 dark:text-red-200">
              {error}
            </div>
          )}

          <div className="mb-3">
            <label className="block text-sm text-gray-700 dark:text-gray-300">Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="seu@email.com"
              autoComplete="email"
              required
              className="mt-1 w-full rounded-xl border border-gray-200 dark:border-slate-700 bg-white dark:bg-slate-950 px-3 py-2 text-sm text-gray-900 dark:text-white outline-none focus:ring-2 focus:ring-brand-500 placeholder:text-gray-400 dark:placeholder:text-gray-600"
            />
          </div>

          <div className="mb-4">
            <PasswordInput
              label="Senha"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="••••••••"
              autoComplete="current-password"
              required
            />
          </div>

          <button
            type="submit"
            disabled={loading || !email || !password}
            className="w-full rounded-xl bg-brand-600 px-3 py-2 text-white transition hover:bg-brand-500 disabled:cursor-not-allowed disabled:opacity-80"
          >
            {loading ? "Entrando..." : "Entrar"}
          </button>

          <div className="mt-4 text-center text-sm">
            <span className="text-gray-600 dark:text-gray-400">Não tem conta?</span>{" "}
            <button type="button" onClick={() => navigate("/register")} className="text-brand-600 dark:text-brand-400 hover:text-brand-700 dark:hover:text-brand-300 cursor-pointer">
              Criar conta
            </button>
          </div>
        </form>
      </div>

      <div className="relative flex items-center justify-center bg-brand-50 dark:bg-slate-800/50 md:flex-1 md:min-h-[80vh] overflow-visible rounded-r-3xl">
        <img
          src="/img/login-mentoria.png"
          alt="Ilustração de mentoria"
          className="w-[130%] max-w-none object-cover -translate-x-3 pointer-events-none"
          loading="lazy"
        />
      </div>
      
    </div>
  </div>
);

}
