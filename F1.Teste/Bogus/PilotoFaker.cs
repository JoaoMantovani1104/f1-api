using Bogus;
using F1.Lib.Modelos;

namespace F1.Teste.Bogus;

public static class PilotoFaker
{
    public static Faker<Piloto> Gerar()
    {
        return new Faker<Piloto>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Nome, f => f.Name.FullName())
            .RuleFor(p => p.Numero, f => f.Random.Int(1, 99))
            .RuleFor(p => p.Idade, f => f.Random.Int(17, 45))
            .RuleFor(p => p.Nacionalidade, f => f.Address.Country())
            .RuleFor(p => p.EquipeId, f => f.Random.Int(1, 10));
    }
}
