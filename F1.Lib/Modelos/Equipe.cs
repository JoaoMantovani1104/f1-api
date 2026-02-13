namespace F1.Lib.Modelos;

public class Equipe
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public virtual ICollection<Piloto> Pilotos { get; set; } = [];
}
