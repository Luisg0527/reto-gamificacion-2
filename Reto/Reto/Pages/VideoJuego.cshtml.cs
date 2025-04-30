using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reto.Model;
using Microsoft.AspNetCore.Http;

namespace Reto.Pages
{
    public class VideoJuegoModel : PageModel
    {
        public readonly DataBaseContext _context;
        public List<Controles> Controles { get; set; } = new List<Controles>();
        public List<Creditos> Creditos { get; set; } = new List<Creditos>();
        public List<Licencias> Licencias { get; set; } = new List<Licencias>();
        public List<Objetivo> Objetivo { get; set; } = new List<Objetivo>();
        public List<Videojuego> Videojuego { get; set; } = new List<Videojuego>();

        public VideoJuegoModel(DataBaseContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Controles = _context.obtenerControles();
            Creditos = _context.obtenerCreditos();
            Licencias = _context.obtenerLicencias();
            Objetivo = _context.obtenerObjetivo();
            Videojuego = _context.obtenerVideojuego();
        }

    }
}