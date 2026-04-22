import { api } from "./gateway";

export interface Mentor {
  id: number;
  nome: string;
  descricao: string;
  area?: string;
  tecnologias?: string;
  mentorId?: number;
}

export async function listarMentores(filtro?: string): Promise<Mentor[]> {
  const q = filtro?.trim();
  const response = await api.get("/cadastro/mentor", {
    params: q ? { filtro: q } : undefined,
  });
  return response.data;
}

export async function cadastrarMentor(mentor: Omit<Mentor, 'id'>): Promise<Mentor> {
  const response = await api.post("/cadastro/mentor", mentor);
  return response.data;
}

// Semantica
export async function listarMentoresBuscaSemantica(filtro?: string): Promise<string> {
  const q = filtro?.trim();
  const response = await api.get("/busca/buscaSemantica", {
    params: q ? { query: q } : undefined,
  });
  return response.data;
}

// IR Search
export async function listarMentoresBuscaIR(filtro: string): Promise<Mentor[]> {
  const q = filtro?.trim();
  const response = await api.get("/buscair/search", {
    params: q ? { q: q } : undefined,
  });
  return response.data;
}