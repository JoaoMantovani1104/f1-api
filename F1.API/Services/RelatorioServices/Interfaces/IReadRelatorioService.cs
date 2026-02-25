using F1.API.Data.Dtos.RelatorioDTO;

namespace F1.API.Services.RelatorioServices.Interfaces;

public interface IReadRelatorioService
{
    Task<RelatorioDTO> LerRelatorioGeralAsync();
}
