using F1.Lib.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F1.API.Data.Configuracoes;

public class EquipeConfiguration : IEntityTypeConfiguration<Equipe>
{
    public void Configure(EntityTypeBuilder<Equipe> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(e => e.Pilotos)
            .WithOne(p => p.Equipe)
            .HasForeignKey(p => p.EquipeId);
    }
}