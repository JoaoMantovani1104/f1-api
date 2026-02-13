using F1.Lib.Modelos;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace F1.API.Data;

public class F1Context : DbContext
{
    public F1Context(DbContextOptions<F1Context> options) : base(options) { }
    public DbSet<Piloto> Pilotos { get; set; }
    public DbSet<Equipe> Equipes { get; set; }
    public DbSet<GrandePremio> GrandesPremios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());   
    }
}
