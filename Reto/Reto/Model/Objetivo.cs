public class Objetivo
{
    public int Id { get; set; }
    public int VideojuegoId { get; set; }
    public string? Tipo { get; set; }  // "ganar" o "perder"
    public string? Descripcion { get; set; }
}
