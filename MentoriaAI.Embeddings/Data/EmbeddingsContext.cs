using Microsoft.EntityFrameworkCore;
using Pgvector;
using MentoriaAI.Embeddings.Models;

public class EmbeddingsContext : DbContext
{
    public EmbeddingsContext(DbContextOptions<EmbeddingsContext> options) : base(options)
    {
    }

    public DbSet<MentorEmbedding> MentorEmbeddings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<MentorEmbedding>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Descricao)
                .HasMaxLength(1000);

            entity.Property(e => e.Embedding)
                .HasColumnType("vector(1536)"); // text-embedding-3-small usa 1536 dimensões
        });
    }
}
