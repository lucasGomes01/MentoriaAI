import { Search, Sparkles, Filter } from "lucide-react";
import { Input } from "@/components/ui/input";

interface SearchBarProps {
  query: string;
  setQuery: (val: string) => void;
  searchType: "ir" | "semantica"; // | "tradicional";
  setSearchType: (val: "ir" | "semantica" ) => void; // | "tradicional"
  searching: boolean;
}

export function SearchBar({ query, setQuery, searchType, setSearchType, searching }: SearchBarProps) {
  return (
    <div className="bg-white/80 dark:bg-slate-900/80 backdrop-blur-md rounded-2xl shadow-lg border border-gray-100 dark:border-slate-800 p-6 transition-colors duration-300">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold text-gray-800 dark:text-white tracking-tight">Encontre seu Mentor</h1>
        {searching && (
          <span className="text-xs font-medium text-brand-600 dark:text-brand-400 bg-brand-50 dark:bg-brand-950/50 px-3 py-1 rounded-full animate-pulse flex items-center gap-2">
            <div className="w-2 h-2 bg-brand-500 dark:bg-brand-400 rounded-full"></div> Buscando...
          </span>
        )}
      </div>

      <div className="flex flex-col lg:flex-row gap-4">
        <div className="relative flex-grow">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 w-5 h-5" />
          <Input
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            placeholder="O que você procura? Ex: Especialista em React..."
            className="pl-10 h-12 text-base rounded-xl border-gray-200 dark:border-slate-700 bg-white dark:bg-slate-950 text-gray-900 dark:text-white focus:border-brand-500 focus:ring-brand-500 shadow-sm placeholder:text-gray-400 dark:placeholder:text-gray-500 transition-colors"
          />
        </div>

        <div className="flex bg-gray-100/80 dark:bg-slate-800/80 p-1 rounded-xl shadow-inner overflow-x-auto hide-scrollbar">
          {/* <button
            className={`px-4 py-2 rounded-lg text-sm font-semibold transition-all whitespace-nowrap ${searchType === "tradicional" ? "bg-white dark:bg-slate-700 text-gray-800 dark:text-white shadow-sm" : "text-gray-500 dark:text-gray-400 hover:text-gray-700 dark:hover:text-gray-200 hover:bg-gray-200/50 dark:hover:bg-slate-700/50"}`}
            onClick={() => setSearchType("tradicional")}
          >
            Tradicional
          </button> */}
          <button
            className={`px-4 py-2 rounded-lg text-sm font-semibold transition-all whitespace-nowrap flex items-center gap-1 ${searchType === "ir" ? "bg-white dark:bg-slate-700 text-brand-600 dark:text-brand-400 shadow-sm" : "text-gray-500 dark:text-gray-400 hover:text-gray-700 dark:hover:text-gray-200 hover:bg-gray-200/50 dark:hover:bg-slate-700/50"}`}
            onClick={() => setSearchType("ir")}
          >
            <Filter className="w-4 h-4" /> Inteligente (IR)
          </button>
          <button
            className={`px-4 py-2 rounded-lg text-sm font-semibold transition-all whitespace-nowrap flex items-center gap-1 ${searchType === "semantica" ? "bg-gradient-to-r from-purple-500 to-indigo-500 text-white shadow-md" : "text-gray-500 dark:text-gray-400 hover:text-gray-700 dark:hover:text-gray-200 hover:bg-gray-200/50 dark:hover:bg-slate-700/50"}`}
            onClick={() => setSearchType("semantica")}
          >
            <Sparkles className="w-4 h-4" /> Semântica (IA)
          </button>
        </div>
      </div>
    </div>
  );
}
