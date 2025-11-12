using Microsoft.AspNetCore.Mvc;
using MentoriaAI.Cadastro.Models;
using MentoriaAI.Cadastro.Services;

namespace MentoriaAI.Cadastro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MentorController : ControllerBase
    {
        private readonly MentorService _service;

        public MentorController(MentorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var mentores = await _service.ObterTodosAsync();
            return Ok(mentores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var mentor = await _service.ObterPorIdAsync(id);
            return mentor == null ? NotFound() : Ok(mentor);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Mentor mentor)
        {
            var criado = await _service.CriarMentorAsync(mentor);
            return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Mentor mentor)
        {
            if (id != mentor.Id) return BadRequest();

            var atualizado = await _service.AtualizarMentorAsync(mentor);
            return atualizado ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removido = await _service.DeletarMentorAsync(id);
            return removido ? NoContent() : NotFound();
        }
    }
}
