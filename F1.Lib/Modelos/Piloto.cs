namespace F1.Lib.Modelos;

public class Piloto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Numero { get; set; }
    public int Idade { get; set; }
    public string Nacionalidade { get; set; }
    public int EquipeId { get; set; }
    public virtual Equipe? Equipe { get; set; }
    public List<GrandePremio> GpsVencidos { get; set; } = [];
}
