using F1.Lib.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F1.API.Data.Configuracoes;

public class PilotoConfiguration : IEntityTypeConfiguration<Piloto>
{
    public void Configure(EntityTypeBuilder<Piloto> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Numero)
            .IsRequired();

        builder.Property(p => p.Nacionalidade)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(p => p.Equipe)
            .WithMany(e => e.Pilotos)
            .HasForeignKey(p => p.EquipeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}