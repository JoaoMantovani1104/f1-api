using Bogus;
using F1.Lib.Modelos;

namespace F1.Teste.Bogus;

public static class GPFaker
{
    public static Faker<GrandePremio> Gerar()
    {
        var dadosReais = new[]
        {
            ("GP do Bahrein", "Circuito Internacional do Bahrein", "Bahrein"),
            ("GP da Arábia Saudita", "Circuito Corniche de Gidá", "Arábia Saudita"),
            ("GP da Austrália", "Albert Park", "Austrália"),
            ("GP do Japão", "Suzuka", "Japão"),
            ("GP de Miami", "Autódromo Internacional de Miami", "EUA"),
            ("GP de Mônaco", "Circuito de Mônaco", "Mônaco"),
            ("GP do Brasil", "Autódromo de Interlagos", "Brasil"),
            ("GP da Itália", "Monza", "Itália"),
            ("GP da Inglaterra", "Silverstone", "Reino Unido")
        };

        var pilotoFaker = PilotoFaker.Gerar();

        var vencedor = pilotoFaker.Generate();

        return new Faker<GrandePremio>()
            .CustomInstantiator(f =>
            {
                var (nomeGp, nomeCircuito, nomePais) = f.PickRandom(dadosReais);
                return new GrandePremio
                {
                    Id = f.IndexFaker + 1,
                    Nome = nomeGp + " - " + nomeCircuito,
                    Localizacao = nomePais,
                    Voltas = f.Random.Int(50, 78),
                    Ordem = f.Random.Int(1, 25),
                    VencedorId = vencedor.Id,
                    Vencedor = vencedor
                };
            });
            
    }
}
