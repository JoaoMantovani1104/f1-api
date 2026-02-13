namespace F1.Lib.Modelos;

public class GrandePremio
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Localizacao { get; set; }
    public int Voltas { get; set; }
    public int Ordem { get; set; }
    public int? VencedorId { get; set; }
    public Piloto? Vencedor { get; set; }
}
