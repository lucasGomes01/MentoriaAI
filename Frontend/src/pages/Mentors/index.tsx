import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { listarMentores, type Mentor } from "@/api/mentores";
import { useAuth } from "@/auth/AuthContext";
import { supabase } from "@/auth/supabase";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Label } from "@/components/ui/label";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Separator } from "@/components/ui/separator";
import { Skeleton } from "@/components/ui/skeleton";
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert";

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

export default function MentoresPage() {
  const [mentores, setMentores] = useState<Mentor[]>([]);
  const [loading, setLoading] = useState(true);
  const [erro, setErro] = useState("");
  const [query, setQuery] = useState("");
  const { user, loading: authLoading } = useAuth();
  const navigate = useNavigate();

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

  useEffect(() => {
    async function carregar() {
      try {
        const lista = await listarMentores();
        setMentores(lista);
      } catch (err: any) {
        setErro("Erro ao carregar mentores");
      } finally {
        setLoading(false);
      }
    }

    carregar();
  }, []);

  if (loading)
    return (
      <div className="min-h-screen bg-gradient-to-br from-brand-50 to-white">
        <div className="max-w-6xl mx-auto px-6 py-10">
          <div className="bg-white/60 backdrop-blur rounded-2xl shadow-xl p-6">
            <div className="flex items-center justify-between mb-6">
              <div className="flex items-center gap-3">
                <Skeleton className="h-9 w-9 rounded-full" />
                <div>
                  <Skeleton className="h-4 w-24" />
                  <Skeleton className="mt-2 h-5 w-40" />
                </div>
              </div>
              <Skeleton className="h-9 w-24" />
            </div>
            <Skeleton className="h-6 w-32" />
            <Skeleton className="mt-2 h-4 w-64" />
            <Skeleton className="mt-4 h-10 w-80" />
          </div>
          <div className="mt-6 grid grid-cols-[repeat(auto-fill,minmax(320px,1fr))] gap-5">

            {Array.from({ length: 6 }).map((_, i) => (
              <div key={i} className="p-5 bg-white/60 backdrop-blur rounded-2xl border shadow-sm">
                <div className="flex items-center gap-4">
                  <Skeleton className="h-14 w-14 rounded-full" />
                  <div className="flex-1">
                    <Skeleton className="h-5 w-48" />
                    <Skeleton className="mt-2 h-4 w-64" />
                    <Skeleton className="mt-2 h-4 w-36" />
                  </div>
                </div>
              </div>
            ))}
            
          </div>
        </div>
      </div>
    );

  if (erro)
    return (
      <div className="max-w-5xl mx-auto px-6 py-8">
        <Alert variant="destructive">
          <AlertTitle>Erro ao carregar</AlertTitle>
          <AlertDescription>{erro}</AlertDescription>
        </Alert>
      </div>
    );

  return (
    <div className="min-h-screen bg-gradient-to-br from-brand-50 to-white">
      <div className="max-w-6xl mx-auto px-6 py-10">
        <div className="bg-gradient-to-br from-brand-600 to-brand-400 text-white rounded-2xl p-6 shadow-xl">
          <div className="flex items-center justify-between">
            {user ? (
              <div className="flex items-center gap-3">
            <Avatar className="w-9 h-9" style={{ background: user?.email ? colorFromString(user.email) : "#2563eb" }}>
              <AvatarFallback className="text-white font-bold text-xs">
                {getInicials(displayName)}
              </AvatarFallback>
            </Avatar>
            <div>
              <div className="text-sm/relaxed text-white/80">Bem-vindo</div>
              <div className="text-base font-semibold">
                {authLoading ? "Carregando..." : displayName}
              </div>
            </div>
              </div>
            ) : (
              <div>
            <div className="text-base font-semibold">MentoriaAI</div>
            <div className="text-sm text-white/80">Descubra mentores e comece agora</div>
              </div>
            )}

            {user ? (
          <Button className="bg-white/20 hover:bg-white/30 text-white border-white/30" variant="outline" onClick={signOut}>Sair</Button>
            ) : (
          <Button className="bg-white/20 hover:bg-white/30 text-white border-white/30" onClick={() => navigate("/login")}>Entrar</Button>
            )}
          </div>
        </div>

        <div className="mt-6 bg-white/70 backdrop-blur rounded-2xl shadow-xl p-6">
          <h1 className="m-0 text-2xl font-semibold text-foreground">Mentores</h1>
          <p className="mt-2 text-muted-foreground">Lista de profissionais cadastrados para mentoria.</p>
          <div className="mt-4 max-w-md">
            <Label htmlFor="search">Busca</Label>
            <div className="relative">
              <Input
                id="search"
                value={query}
                onChange={(e) => setQuery(e.target.value)}
                placeholder="Buscar por nome ou descrição"
                className="pl-9"
              />
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" className="absolute left-2 top-1/2 -translate-y-1/2 size-4 text-muted-foreground">
                <path d="M21 21l-4.3-4.3" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"></path>
                <circle cx="11" cy="11" r="8" strokeWidth="2"></circle>
              </svg>
            </div>
          </div>
          <Separator className="mt-6" />
        </div>

        {mentores.length === 0 && (
          <Card className="bg-white/70 backdrop-blur rounded-2xl shadow-xl">
            <CardContent className="p-5 text-sm text-muted-foreground">Nenhum mentor encontrado.</CardContent>
          </Card>
        )}

        <ul className="p-0 list-none grid grid-cols-[repeat(auto-fill,minmax(320px,1fr))] gap-5 mt-6">
        {(query ? mentores.filter((m) => {
          const q = query.toLowerCase();
          return (
            m.nome.toLowerCase().includes(q) ||
            (m.descricao ? m.descricao.toLowerCase().includes(q) : false)
          );
        }) : mentores).map((m) => (
          <li key={m.id}>
            <Card className="bg-white/70 backdrop-blur rounded-2xl border shadow-sm transition hover:shadow-lg hover:-translate-y-0.5">
              <CardContent className="p-5">
                <div className="flex items-center gap-4">
                  <Avatar className="w-14 h-14" style={{ background: colorFromString(m.nome) }}>
                    <AvatarFallback className="text-white font-bold text-base">
                      {getInicials(m.nome)}
                    </AvatarFallback>
                  </Avatar>
                  <div className="flex-1">
                    <div className="flex items-baseline">
                      <h3 className="m-0 text-lg font-semibold text-foreground">{m.nome}</h3>
                    </div>
                    {m.descricao && (
                      <p className="mt-2 text-sm text-muted-foreground">{m.descricao}</p>
                    )}
                    {m.mentorId && (
                      <div className="mt-2">
                        <Badge className="bg-brand-100 text-brand-700"> Código interno: {m.mentorId} </Badge>
                      </div>
                    )}
                  </div>
                </div>
              </CardContent>
            </Card>
          </li>
        ))}
        </ul>
      </div>
    </div>
  );
}
