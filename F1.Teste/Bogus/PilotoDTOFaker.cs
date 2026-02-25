using Bogus;
using F1.API.Data.Dtos.PilotoDTO;

namespace F1.Teste.Bogus;

public class PilotoDTOFaker
{
    public static Faker<CreatePilotoDTO> GerarCreate()
    {
        return new Faker<CreatePilotoDTO>()
            .RuleFor(p => p.Nome, f => f.Name.FullName())
            .RuleFor(p => p.Numero, f => f.Random.Int(1, 99))
            .RuleFor(p => p.Idade, f => f.Random.Int(17, 45))
            .RuleFor(p => p.Nacionalidade, f => f.Address.Country())
            .RuleFor(p => p.EquipeId, f => f.Random.Int(1, 10));
    }

    public static Faker<ReadPilotoDTO> GerarRead()
    {
        return new Faker<ReadPilotoDTO>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Nome, f => f.Name.FullName())
            .RuleFor(p => p.Numero, f => f.Random.Int(1, 99))
            .RuleFor(p => p.Nacionalidade, f => f.Address.Country())
            .RuleFor(p => p.NomeEquipe, f => f.Vehicle.Manufacturer());
    }

    public static Faker<UpdatePilotoDTO> GerarUpdate()
    {
        return new Faker<UpdatePilotoDTO>()
            .RuleFor(p => p.Nome, f => f.Name.FullName())
            .RuleFor(p => p.Numero, f => f.Random.Int(1, 99))
            .RuleFor(p => p.Idade, f => f.Random.Int(17, 45))
            .RuleFor(p => p.Nacionalidade, f => f.Address.Country())
            .RuleFor(p => p.EquipeId, f => f.Random.Int(1, 10));
    }
}
