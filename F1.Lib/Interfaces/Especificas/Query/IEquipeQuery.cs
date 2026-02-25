using F1.Lib.Modelos;
using F1.Lib.Interfaces.Genericas;

namespace F1.Lib.Interfaces.Especificas.Query;

public interface IEquipeQuery : IQueryBase<Equipe>
{
    Task<bool> ExistemPilotosNaEquipeAsync(int equipeId);
}
