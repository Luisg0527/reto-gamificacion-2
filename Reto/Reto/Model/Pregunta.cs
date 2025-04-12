using Reto.Model;

public class Pregunta
{
    public int id_pregunta { get; set; }
    public string? nombre_usuario { get; set; }
    public string? texto_pregunta { get; set; }
    public DateTime date { get; set; } = DateTime.Today;
    public int likes { get; set; }

    public List<Respuesta> Respuestas { get; set; }


    public Pregunta()
    {
        // Constructor por defecto
    }
}