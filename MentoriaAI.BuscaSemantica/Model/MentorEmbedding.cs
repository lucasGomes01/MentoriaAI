using Pgvector;

namespace MentoriaAI.BuscaSemantica.Models
{
	public class MentorEmbedding
	{
		public int Id { get; set; }
		public int MentorId { get; set; }
		public string Nome { get; set; } = string.Empty;
		public string Descricao { get; set; } = string.Empty;
		public Vector Embedding { get; set; }
	}
}
