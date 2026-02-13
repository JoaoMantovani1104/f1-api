using F1.Lib.Modelos;
using F1.API.Services.Relatorio;

namespace F1.Teste
{
    public class RelatorioServiceTests
    {
        private RelatorioService service;

        public RelatorioServiceTests()
        {
            service = new RelatorioService();
        }

        [Fact]
        public void GerarRelatorio_QuandoNaoHouverEquipes_DeveRetornarEquipeVencedoraNA()
        {
            var pilotos = new List<Piloto> {
                new Piloto { Nome = "Senna", Idade = 34, GpsVencidos = new List<GrandePremio> (new GrandePremio[41]) },
                new Piloto { Nome = "Bortoleto", Idade = 21, GpsVencidos = new List<GrandePremio> (new GrandePremio[5]) },
            };
            var equipes = new List<Equipe>();

            var relatorio = service.GerarRelatorio(pilotos, equipes, 46);

            Assert.Equal("N/A", relatorio.EquipeMaisVencedora);
        }

        [Fact]
        public void GerarRelatorio_QuandoNaoHouverPilotos_DeveRetornarMediaIdadeZero()
        {
            var pilotos = new List<Piloto>();
            var equipes = new List<Equipe>();

            var relatorio = service.GerarRelatorio(pilotos, equipes, 46);

            Assert.Equal(0, relatorio.MediaIdade);
        }

        [Fact]
        public void GerarRelatorio_ComDadosValidos_DeveRetornarRelatorioCompletoECorreto()
        {   
            
            var senna = new Piloto { Nome = "Senna", Idade = 34, GpsVencidos = new List<GrandePremio>(new GrandePremio[41]) };
            var bortoleto = new Piloto { Nome = "Bortoleto", Idade = 21, GpsVencidos = new List<GrandePremio>(new GrandePremio[5]) };
            var pilotos = new List<Piloto> { senna, bortoleto };
            var equipes = new List<Equipe>
            {
                new Equipe { Nome = "McLaren", Pilotos = new List<Piloto> {senna } },
                new Equipe { Nome = "Audio", Pilotos = new List<Piloto> {bortoleto} }
            };

            var relatorio = service.GerarRelatorio(pilotos, equipes, 46);

            Assert.Equal(2, relatorio.TotalPilotos);
            Assert.Equal(2, relatorio.TotalEquipes);    
            Assert.Equal(46, relatorio.TotalGps);
            Assert.Equal(27.5, relatorio.MediaIdade);
            Assert.Contains("Senna (41 vitórias)", relatorio.PilotoMaisVencedor);
            Assert.Contains("McLaren (41 vitórias)", relatorio.EquipeMaisVencedora);
        }
    }
}
