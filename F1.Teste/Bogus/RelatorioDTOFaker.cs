using Bogus;
using F1.API.Data.Dtos.RelatorioDTO;

namespace F1.Teste.Bogus;

public static class RelatorioDTOFaker
{
    public static Faker<RelatorioDTO> Gerar()
    {
        return new Faker<RelatorioDTO>()
            .RuleFor(r => r.TotalPilotos, f => f.Random.Int(1, 22))
            .RuleFor(r => r.TotalEquipes, f => f.Random.Int(1, 11))
            .RuleFor(r => r.TotalGps, f => f.Random.Int(1, 25))
            .RuleFor(r => r.MediaIdade, f => f.Random.Int(1, 40))
            .RuleFor(r => r.PilotoMaisVencedor, f => f.Name.FullName())
            .RuleFor(r => r.EquipeMaisVencedora, f => f.Vehicle.Manufacturer());
    }
}
