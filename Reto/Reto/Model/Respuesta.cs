public class Respuesta
{
    public int id_respuesta { get; set; }
   public string? nombre_usuario { get; set; }
    public string? texto_respuesta { get; set; }
     public DateTime date { get; set; } = DateTime.Today;
    public int likes { get; set; }
    public Respuesta(int id_respuesta_, string? nombre_usuario_, string? texto_respuesta_, int likes_)
    {
        this.id_respuesta = id_respuesta_;
        this.nombre_usuario = nombre_usuario_;
        this.texto_respuesta = texto_respuesta_;
        this.likes = likes_;
    }

    public Respuesta()
    {
        // Constructor por defecto
    }
}