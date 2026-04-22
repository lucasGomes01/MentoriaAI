import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Search, Briefcase, Sparkles, Filter } from "lucide-react";
import { listarMentores, listarMentoresBuscaSemantica, listarMentoresBuscaIR, type Mentor } from "@/api/mentores";
import { useAuth } from "@/auth/AuthContext";
import { supabase } from "@/auth/supabase";
import { Card, CardContent } from "@/components/ui/card";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertDescription } from "@/components/ui/alert";
import { Header } from "@/components/layout/Header";
import { SearchBar } from "@/components/mentors/SearchBar";

type MentorSemantico = {
  id: number;
  descricao: string;
  nome: string;
  motivo: string;
};

type RespostaSemantica = {
  mentores: MentorSemantico[];
  resumo: string;
};

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

function useDebounce<T>(value: T, delay: number): T {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);
    return () => clearTimeout(handler);
  }, [value, delay]);

  return debouncedValue;
}

export default function MentoresPage() {
  const [filteredMentores, setFilteredMentores] = useState<Mentor[]>([]);
  const [resumoChat, setResumoChat] = useState("");
  const [mentoresSemanticos, setMentoresSemanticos] = useState<MentorSemantico[]>([]);
  const [loading, setLoading] = useState(true);
  const [searching, setSearching] = useState(false);
  const [erro, setErro] = useState("");
  const [query, setQuery] = useState("");
  const [searchType, setSearchType] = useState<"ir" | "semantica" | "tradicional">("tradicional");

  const { user, loading: authLoading } = useAuth();
  const navigate = useNavigate();
  const debouncedQuery = useDebounce(query, 500);

  const displayName = (
    (user?.user_metadata && (user.user_metadata.name || user.user_metadata.full_name)) ||
    user?.email ||
    "Usuário"
  ) as string;

  const signOut = async () => {
    await supabase.auth.signOut();
    localStorage.removeItem("access_token");
    navigate("/login");
  };

  const executeSearch = async (searchTerm: string, type: typeof searchType) => {
    const q = searchTerm.toLowerCase();

    try {
      setSearching(true);
      setErro("");

      if (type === "semantica") {
        setFilteredMentores([]);
      } else {
        setResumoChat("");
        setMentoresSemanticos([]);
      }

      if (!q.trim() && type !== "tradicional") {
        setSearching(false);
        setResumoChat("");
        setMentoresSemanticos([]);
        return;
      }

      switch (type) {
        case "tradicional": {
          const res = await listarMentores(q);
          setFilteredMentores(Array.isArray(res) ? res : []);
          break;
        }

        case "ir": {
          const res = await listarMentoresBuscaIR(q);
          setFilteredMentores(Array.isArray(res) ? res : []);
          break;
        }

        case "semantica": {
          const res = await listarMentoresBuscaSemantica(q);
          console.log("Resposta da API:", res);
          let data: RespostaSemantica;

          try {
            data = typeof res === "string" ? JSON.parse(res) : res;
          } catch {
            throw new Error("Resposta inválida da API");
          }

          const mentores = Array.isArray(data?.mentores)
            ? data.mentores
            : [];

          setResumoChat(data?.resumo || "");
          setMentoresSemanticos(mentores);

          console.log("testes", data?.mentores);

          break;
        }
      }
    } catch (err) {
      setErro("Falha na busca. Tente novamente.");
      setResumoChat("");
      setMentoresSemanticos([]);
      setFilteredMentores([]);
    } finally {
      setSearching(false);
      setLoading(false);
    }
  };

  useEffect(() => {
    executeSearch("", "tradicional");
  }, []);

  useEffect(() => {
    if (!debouncedQuery.trim()) return;
    executeSearch(debouncedQuery, searchType);
  }, [debouncedQuery, searchType]);

  if (loading && !debouncedQuery) {
    return (
      <div className="min-h-screen bg-brand-50 dark:bg-slate-950 transition-colors duration-300 p-10">
        <Skeleton className="h-16 w-full max-w-6xl mx-auto mb-10 rounded-2xl dark:bg-slate-800" />
        <div className="max-w-6xl mx-auto grid grid-cols-1 md:grid-cols-2 gap-6">
          <Skeleton className="h-40 w-full rounded-2xl dark:bg-slate-800" />
          <Skeleton className="h-40 w-full rounded-2xl dark:bg-slate-800" />
        </div>
      </div>
    );
  }
console.log({ mentoresSemanticos });  
  return (
    <div className="min-h-screen bg-gradient-to-br from-brand-50 to-white dark:from-slate-950 dark:to-slate-900 transition-colors duration-300">
      <Header 
        displayName={displayName}
        userEmail={user?.email}
        authLoading={authLoading}
        onSignOut={signOut}
      />

      <div className="max-w-6xl mx-auto px-6 py-10">
        <SearchBar 
          query={query}
          setQuery={setQuery}
          searchType={searchType}
          setSearchType={setSearchType}
          searching={searching}
        />

        {erro && (
          <Alert variant="destructive" className="mt-6 dark:bg-red-950/50 dark:border-red-900">
            <AlertDescription className="dark:text-red-200">{erro}</AlertDescription>
          </Alert>
        )}

        {/* RESULTADOS */}
        <div className="mt-6">

          {searchType === "semantica" ? (
            <>
              {/* CHAT */}
              <Card className="border-transparent shadow-lg bg-gradient-to-r from-purple-50 to-indigo-50/50 dark:from-slate-800 dark:to-indigo-950/30 backdrop-blur-sm overflow-hidden relative">
                <div className="absolute -right-10 -top-10 opacity-5 dark:opacity-10">
                  <Sparkles className="w-40 h-40" />
                </div>
                <CardContent className="p-6 relative z-10">
                  {!resumoChat && !searching ? (
                    <div className="text-center py-8">
                      <div className="w-16 h-16 bg-white dark:bg-slate-800 rounded-full flex items-center justify-center mx-auto mb-4 shadow-sm border border-gray-100 dark:border-slate-700">
                        <Sparkles className="w-8 h-8 text-purple-400 dark:text-purple-300" />
                      </div>
                      <h3 className="text-lg font-semibold text-gray-700 dark:text-gray-200 mb-1">Como a IA pode ajudar?</h3>
                      <p className="text-gray-500 dark:text-gray-400 text-sm max-w-md mx-auto">
                        Descreva seu desafio atual ou objetivo de carreira. A IA encontrará os mentores ideais e explicará o porquê.
                      </p>
                    </div>
                  ) : (
                    <div className="space-y-6">
                      <div className="flex justify-end">
                        <div className="bg-purple-600 dark:bg-purple-500 text-white px-5 py-3 rounded-2xl rounded-tr-sm shadow-md max-w-[85%] text-sm leading-relaxed">
                          {debouncedQuery}
                        </div>
                      </div>

                      <div className="flex justify-start">
                        <div className="bg-white dark:bg-slate-800 px-5 py-4 rounded-2xl rounded-tl-sm shadow-md border border-purple-100 dark:border-slate-700 max-w-[90%] text-sm text-gray-700 dark:text-gray-200 leading-relaxed">
                          {searching ? (
                            <div className="flex items-center gap-2">
                              <Sparkles className="w-4 h-4 text-purple-500 dark:text-purple-400 animate-spin" />
                              <span className="animate-pulse dark:text-gray-300">Analisando perfis e compatibilidade...</span>
                            </div>
                          ) : (
                            resumoChat
                          )}
                        </div>
                      </div>
                    </div>
                  )}
                </CardContent>
              </Card>

              {/* CARDS SEMANTICOS */}
              {Array.isArray(mentoresSemanticos) && mentoresSemanticos.length > 0 && (
                <div className="mt-8 grid grid-cols-1 md:grid-cols-2 gap-6">
                  {mentoresSemanticos.map((m) => (
                    <Card key={m.id} className="group border-transparent shadow-md hover:shadow-2xl hover:-translate-y-1 transition-all duration-300 bg-gradient-to-br from-white/80 to-purple-50/50 dark:from-slate-800/80 dark:to-indigo-900/30 backdrop-blur-md overflow-hidden relative flex flex-col h-full">
                      <div className="absolute top-0 right-0 p-4 opacity-5 group-hover:opacity-10 transition-opacity">
                        <Sparkles className="w-16 h-16 text-purple-500 dark:text-purple-400" />
                      </div>
                      <CardContent className="p-6 flex flex-col flex-grow gap-4 relative z-10">
                        <div className="flex gap-4 items-start">
                          <Avatar className="w-14 h-14 ring-2 ring-white dark:ring-slate-700 shadow-sm" style={{ background: colorFromString(m.nome) }}>
                            <AvatarFallback className="text-white text-lg">{getInicials(m.nome)}</AvatarFallback>
                          </Avatar>
                          <div>
                            <h3 className="font-bold text-lg text-gray-800 dark:text-gray-100 leading-tight group-hover:text-purple-600 dark:group-hover:text-purple-400 transition-colors">{m.nome}</h3>
                          </div>
                        </div>
                        
                        <p className="text-sm text-gray-600 dark:text-gray-300 leading-relaxed italic border-l-2 border-purple-200 dark:border-purple-800/50 pl-3 flex-grow">
                          "{m.descricao}"
                        </p>

                        <div className="mt-2 pt-4 border-t border-purple-100/50 dark:border-slate-700/50">
                          <h4 className="text-xs font-bold text-purple-700 dark:text-purple-400 uppercase tracking-wider mb-2 flex items-center gap-1">
                            <Sparkles className="w-3 h-3" /> Por que este mentor?
                          </h4>
                          <p className="text-sm text-gray-700 dark:text-gray-300 leading-relaxed">
                            {m.motivo}
                          </p>
                        </div>
                      </CardContent>
                    </Card>
                  ))}
                </div>
              )}
            </>
          ) : (
            <div className={filteredMentores.length === 0 && !searching && debouncedQuery ? "text-center py-12" : "grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"}>
              {filteredMentores.length === 0 && !searching && debouncedQuery ? (
                <div className="col-span-full">
                  <div className="w-20 h-20 bg-gray-100 dark:bg-slate-800 rounded-full flex items-center justify-center mx-auto mb-4 border border-gray-200 dark:border-slate-700">
                    <Search className="w-10 h-10 text-gray-400 dark:text-gray-500" />
                  </div>
                  <h3 className="text-lg font-semibold text-gray-800 dark:text-gray-200">Nenhum mentor encontrado</h3>
                  <p className="text-gray-500 dark:text-gray-400 mt-2">Tente buscar por outras palavras-chave ou utilize a busca Semântica (IA).</p>
                </div>
              ) : (
                filteredMentores.map((m) => (
                  <Card key={m.id} className="group border-transparent shadow-md hover:shadow-2xl hover:-translate-y-1 transition-all duration-300 bg-white/80 dark:bg-slate-800/80 backdrop-blur-md overflow-hidden flex flex-col h-full border-gray-100 dark:border-slate-700/50">
                    <CardContent className="p-6 flex flex-col flex-grow gap-4">
                      <div className="flex gap-4 items-start">
                        <Avatar className="w-14 h-14 ring-2 ring-white dark:ring-slate-700 shadow-sm" style={{ background: colorFromString(m.nome) }}>
                          <AvatarFallback className="text-white text-lg">{getInicials(m.nome)}</AvatarFallback>
                        </Avatar>
                        <div>
                          <h3 className="font-bold text-lg text-gray-800 dark:text-gray-100 leading-tight group-hover:text-brand-600 dark:group-hover:text-brand-400 transition-colors">{m.nome}</h3>
                          {m.area && (
                            <div className="flex items-center gap-1 text-xs text-brand-600 dark:text-brand-400 font-medium mt-1">
                              <Briefcase className="w-3 h-3" />
                              <span className="truncate max-w-[150px]">{m.area}</span>
                            </div>
                          )}
                        </div>
                      </div>
                      
                      <p className="text-sm text-gray-600 dark:text-gray-300 flex-grow leading-relaxed">
                        {m.descricao}
                      </p>

                      {m.tecnologias && (
                        <div className="flex flex-wrap gap-2 mt-4 pt-4 border-t border-gray-100 dark:border-slate-700">
                          {m.tecnologias.split(',').slice(0, 4).map((tech, i) => (
                            <span key={i} className="bg-brand-50 dark:bg-brand-900/30 text-brand-700 dark:text-brand-300 px-2.5 py-1 rounded-md text-xs font-semibold flex items-center gap-1">
                              {tech.trim()}
                            </span>
                          ))}
                          {m.tecnologias.split(',').length > 4 && (
                            <span className="text-xs text-gray-400 font-medium self-center">+{m.tecnologias.split(',').length - 4}</span>
                          )}
                        </div>
                      )}
                    </CardContent>
                  </Card>
                ))
              )}
            </div>
          )}

        </div>
      </div>
    </div>
  );
}