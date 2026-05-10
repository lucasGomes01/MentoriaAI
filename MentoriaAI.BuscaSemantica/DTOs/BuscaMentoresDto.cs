namespace MentoriaAI.BuscaSemantica.DTOs;

public class MentorSemanticoDto
{
    public decimal Id { get; set; }
    public string Descricao { get; set; }
    public string Nome { get; set; }
    public string Motivo { get; set; }
}

public class BuscaMentoresDto
{
    public List<MentorSemanticoDto> Mentores { get; set; }
    public string Resumo { get; set; }
}
