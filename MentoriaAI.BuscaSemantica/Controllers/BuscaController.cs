using Microsoft.AspNetCore.Mvc;
using MentoriaAI.BuscaSemantica.Services;

namespace MentoriaAI.BuscaSemantica.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuscaController : ControllerBase
    {
        private readonly BuscaSemanticaService _buscaService;

        public BuscaController(BuscaSemanticaService buscaService)
        {
            _buscaService = buscaService;
        }

        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("A consulta não pode ser vazia.");

            var resultados = await _buscaService.BuscarMentoresAsync(query);
            return Ok(resultados);
        }
    }
}
