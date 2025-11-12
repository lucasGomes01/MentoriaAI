namespace MentoriaAI.Contracts.Events
{
    public class MentorAtualizadoEvent
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Tecnologias { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}