using Microsoft.EntityFrameworkCore;
using MentoriaAI.Cadastro.Models;
using System.Collections.Generic;

namespace MentoriaAI.Cadastro.Data;

public class MentoriaContext : DbContext
{
    public MentoriaContext(DbContextOptions<MentoriaContext> options)
    : base(options) { }

    public DbSet<Mentor> Mentores => Set<Mentor>();
}
