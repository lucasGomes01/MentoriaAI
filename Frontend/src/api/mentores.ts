import { api } from "./gateway";

export interface Mentor {
  id: number;
  nome: string;
  descricao: string;
  mentorId?: number;
}

export async function listarMentores(): Promise<Mentor[]> {
  const response = await api.get("/cadastro/mentor");
  return response.data;
}
