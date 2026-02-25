using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;

namespace F1.Lib.Interfaces.Especificas.Query;

public interface IPilotoQuery : IQueryBase<Piloto>
{
    Task<bool> ExistePilotoComNumeroAsync(int numero);
    Task<double> ObterMediaIdade();
}
