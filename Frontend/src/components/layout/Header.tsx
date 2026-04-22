import { Moon, Sun, LogOut, User } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { useTheme } from "@/components/theme/ThemeProvider";

interface HeaderProps {
  displayName: string;
  userEmail: string | undefined;
  authLoading: boolean;
  onSignOut: () => void;
}

function getInicials(nome: string) {
  const partes = nome.trim().split(/\s+/);
  const iniciais = partes.slice(0, 2).map((p) => p[0]).join("");
  return iniciais.toUpperCase();
}

function colorFromString(input: string) {
  let hash = 0;
  for (let i = 0; i < input.length; i++) {
    hash = input.charCodeAt(i) + ((hash << 5) - hash);
  }
  const hue = Math.abs(hash) % 360;
  return `hsl(${hue}, 70%, 60%)`;
}

export function Header({ displayName, userEmail, authLoading, onSignOut }: HeaderProps) {
  const { theme, setTheme } = useTheme();

  return (
    <header className="sticky top-0 z-50 w-full border-b border-gray-200 dark:border-slate-800 bg-white/80 dark:bg-slate-900/80 backdrop-blur-md shadow-sm transition-colors duration-300">
      <div className="max-w-6xl mx-auto px-6 h-16 flex items-center justify-between">
        <div className="flex items-center gap-2">
          <div className="bg-brand-600 dark:bg-brand-500 p-1.5 rounded-lg">
            <User className="w-5 h-5 text-white" />
          </div>
          <span className="font-bold text-lg tracking-tight text-gray-900 dark:text-white">
            Mentoria<span className="text-brand-600 dark:text-brand-400">AI</span>
          </span>
        </div>

        <div className="flex items-center gap-4">
          <Button
            variant="ghost"
            size="icon"
            onClick={() => setTheme(theme === "dark" ? "light" : "dark")}
            className="text-gray-500 hover:text-gray-900 dark:text-gray-400 dark:hover:text-gray-50 rounded-full"
          >
            {theme === "dark" ? <Sun className="h-5 w-5" /> : <Moon className="h-5 w-5" />}
          </Button>

          <div className="hidden sm:flex items-center gap-3 pl-4 border-l border-gray-200 dark:border-slate-700">
            <div className="text-right">
              <div className="text-xs text-gray-500 dark:text-gray-400">Bem-vindo</div>
              <div className="text-sm font-semibold text-gray-900 dark:text-gray-100">
                {authLoading ? "Carregando..." : displayName}
              </div>
            </div>
            <Avatar className="w-9 h-9 ring-2 ring-white dark:ring-slate-800 shadow-sm" style={{ background: userEmail ? colorFromString(userEmail) : "#2563eb" }}>
              <AvatarFallback className="text-white font-bold text-xs">
                {getInicials(displayName)}
              </AvatarFallback>
            </Avatar>
          </div>

          <Button 
            variant="outline" 
            size="sm" 
            onClick={onSignOut} 
            className="ml-2 border-gray-200 dark:border-slate-700 hover:bg-gray-100 dark:hover:bg-slate-800 text-gray-700 dark:text-gray-300 gap-2"
          >
            <LogOut className="w-4 h-4" />
            <span className="hidden sm:inline">Sair</span>
          </Button>
        </div>
      </div>
    </header>
  );
}
