using F1.API.Data.Dtos.RelatorioDTO;
using Microsoft.AspNetCore.Mvc;

namespace F1.API.Services.Relatorio.Interfaces;

public interface IReadRelatorioService
{
    Task<RelatorioDTO> LerRelatorioGeralAsync();
}
