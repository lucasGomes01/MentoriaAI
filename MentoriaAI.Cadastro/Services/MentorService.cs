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

        public async Task<List<Mentor>> ObterTodosAsync()
        {
            return await _context.Mentores.AsNoTracking().ToListAsync();
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
    }
}
