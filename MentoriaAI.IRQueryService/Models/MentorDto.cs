namespace MentoriaAI.IRQueryService.Models;

public class MentorDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Tecnologias { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime DataAtualizacao { get; set; }
}
