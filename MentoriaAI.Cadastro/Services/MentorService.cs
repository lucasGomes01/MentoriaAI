using MassTransit;
using Microsoft.EntityFrameworkCore;
using MentoriaAI.Cadastro.Data;
using MentoriaAI.Contracts.Events;
using MentoriaAI.Cadastro.Models;

namespace MentoriaAI.Cadastro.Services
{
    public class MentorService
    {
        private readonly MentoriaContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public MentorService(MentoriaContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<Mentor>> ObterTodosAsync(string? filtro)
        {
            var query = _context.Mentores.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                var searchTerm = $"%{filtro.Trim()}%";

                query = query.Where(m =>
                    EF.Functions.ILike(m.Nome, searchTerm) ||
                    EF.Functions.ILike(m.Descricao, searchTerm) ||
                    EF.Functions.ILike(m.Tecnologias, searchTerm));
            }

            return await query.OrderBy(m => m.Nome).ToListAsync();
        }

        public async Task<Mentor?> ObterPorIdAsync(int id)
        {
            return await _context.Mentores.FindAsync(id);
        }

        public async Task<Mentor> CriarMentorAsync(Mentor mentor)
        {
            _context.Mentores.Add(mentor);
            await _context.SaveChangesAsync();

            var evento = new MentorCriadoEvent
            {
                Id = mentor.Id,
                Nome = mentor.Nome,
                Area = mentor.Area,
                Tecnologias = mentor.Tecnologias,
                Descricao = mentor.Descricao
            };

            await _publishEndpoint.Publish(evento);
            Console.WriteLine($"[Evento] MentorCriadoEvent publicado: {mentor.Nome}");

            return mentor;
        }

        public async Task<bool> AtualizarMentorAsync(Mentor mentor)
        {
            var existente = await _context.Mentores.FindAsync(mentor.Id);
            if (existente == null) return false;

            _context.Entry(existente).CurrentValues.SetValues(mentor);
            await _context.SaveChangesAsync();

            var evento = new MentorAtualizadoEvent
            {
                Id = mentor.Id,
                Nome = mentor.Nome,
                Area = mentor.Area,
                Tecnologias = mentor.Tecnologias,
                Descricao = mentor.Descricao
            };

            await _publishEndpoint.Publish(evento);
            Console.WriteLine($"[Evento] MentorAtualizadoEvent publicado: {mentor.Nome}");

            return true;
        }

        public async Task<bool> DeletarMentorAsync(int id)
        {
            var mentor = await _context.Mentores.FindAsync(id);
            if (mentor == null) return false;

            _context.Mentores.Remove(mentor);
            await _context.SaveChangesAsync();

            var evento = new MentorDeletadoEvent
            {
                Id = mentor.Id
            };

            await _publishEndpoint.Publish(evento);
            Console.WriteLine($"[Evento] MentorExcluidoEvent publicado: {id}");

            return true;
        }
        public async Task<bool> DeletarTodosMentoresAsync()
        {
            var mentores = await _context.Mentores
                .AsNoTracking()
                .Select(m => new { m.Id })
                .ToListAsync();

            if (!mentores.Any())
                return false;

            foreach (var mentor in mentores)
            {
                var evento = new MentorDeletadoEvent
                {
                    Id = mentor.Id
                };

                await _publishEndpoint.Publish(evento);
                Console.WriteLine($"[Evento] MentorDeletadoEvent publicado: {mentor.Id}");
            }

            // delete em massa depois dos eventos
            await _context.Mentores.ExecuteDeleteAsync();

            Console.WriteLine($"[INFO] Todos os mentores foram removidos. Total: {mentores.Count}");

            return true;
        }
    }
}
