using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;

namespace F1.Lib.Interfaces.Especificas.Query;

public interface IPilotoQuery : IQueryBase<Piloto>
{
    Task<Piloto?> ExistePilotoComNumeroAsync(int numero);
    Task<Piloto?> ObterPilotoComMaisVitoriasAsync();
    Task<double> ObterMediaIdade();
}
