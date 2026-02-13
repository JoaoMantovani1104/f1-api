using F1.Lib.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F1.API.Data.Configuracoes;

public class GpConfiguration : IEntityTypeConfiguration<GrandePremio>
{
    public void Configure(EntityTypeBuilder<GrandePremio> builder)
    {
        builder.HasKey(gp => gp.Id);

        builder.Property(gp => gp.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(gp => gp.Localizacao)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(gp => gp.Vencedor)
            .WithMany(p => p.GpsVencidos)
            .HasForeignKey(gp => gp.VencedorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}