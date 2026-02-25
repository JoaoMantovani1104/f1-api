using Bogus;
using F1.API.Data.Dtos.GrandePremioDTO;

namespace F1.Teste.Bogus;

public static class GrandePremioDTOFaker
{
    public static Faker<CreateGpDTO> GerarCreate()
    {
        return new Faker<CreateGpDTO>()
            .RuleFor(gp => gp.Nome, f => "Grande Prêmio de " + f.Address.City())
            .RuleFor(gp => gp.Localizacao, f => f.Address.Country())
            .RuleFor(gp => gp.Voltas, f => f.Random.Int(45, 78))
            .RuleFor(gp => gp.Ordem, f => f.Random.Int(1, 24));
    }

    public static Faker<ReadGpDTO> GerarRead()
    {
        return new Faker<ReadGpDTO>()
            .RuleFor(g => g.Id, f => f.IndexFaker + 1)
            .RuleFor(gp => gp.Nome, f => "Grande Prêmio de " + f.Address.City())
            .RuleFor(gp => gp.Localizacao, f => f.Address.Country());
    }

    public static Faker<UpdateGpDTO> GerarUpdate()
    {
        return new Faker<UpdateGpDTO>()
            .RuleFor(gp => gp.Nome, f => "Grande Prêmio de " + f.Address.City())
            .RuleFor(gp => gp.Localizacao, f => f.Address.Country())
            .RuleFor(gp => gp.Voltas, f => f.Random.Int(45, 78))
            .RuleFor(gp => gp.Ordem, f => f.Random.Int(1, 24));
    }
}
