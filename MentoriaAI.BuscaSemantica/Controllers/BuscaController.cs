using MentoriaAI.BuscaSemantica.DTOs;
using MentoriaAI.BuscaSemantica.Services;
using Microsoft.AspNetCore.Mvc;

namespace MentoriaAI.BuscaSemantica.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuscaController : ControllerBase
    {
        private readonly BuscaSemanticaService _buscaService;

        public BuscaController(BuscaSemanticaService buscaService)
        {
            _buscaService = buscaService;
        }

        [HttpGet]
        public async Task<ActionResult<BuscaMentoresDto>> Buscar([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("A consulta não pode ser vazia.");

            var resultados = await _buscaService.BuscarMentoresAsync(query);
            return Ok(resultados);
        }
    }
}
