namespace MentoriaAI.Cadastro.Models;

public class Mentor
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Tecnologias { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
