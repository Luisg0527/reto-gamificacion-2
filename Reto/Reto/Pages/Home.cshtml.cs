using Reto.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Reto.Pages;

public class HomeModel : PageModel
{
    public Usuario myUser = new Usuario();
    public MetricasDash metricas = new MetricasDash(0, 0, 0, 0, 0, 0, DateTime.MinValue);
    public int LugarEnLeaderboard { get; set; }
    public Pregunta UltimaPregunta { get; set; }
    public string LookerStudioUrl { get; set; }

    // Conexion a base de datos
    private readonly DataBaseContext _context;
    public HomeModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        string? nombre = HttpContext.Session.GetString("nombreSesion");
        if (nombre != null)
        {
            myUser = _context.GetUserByName(nombre);
            metricas = _context.getMetricas(DateTime.Today);
            
            // Get leaderboard position
            var leaderboard = _context.GetLeaderboard();
            LugarEnLeaderboard = leaderboard.FindIndex(u => u.nombre_usuario == myUser.nombre_usuario) + 1;

            // Get latest forum post
            var preguntas = _context.obtenerPregunta();
            UltimaPregunta = preguntas.OrderByDescending(p => p.date).FirstOrDefault();

            // Get empleadoId from session
            var empleadoId = HttpContext.Session.GetInt32("empleadoIdSesion") ?? -1;
            
            // Construct Looker Studio URL with correct format
            var baseUrl = "https://lookerstudio.google.com/embed/reporting/9c6e7384-63f6-454d-8259-9bc2a128fee0/page/nliIF";
            var paramsJson = $"{{\"idusr\":{empleadoId}}}";
            var encodedParams = HttpUtility.UrlEncode(paramsJson);
            LookerStudioUrl = $"{baseUrl}?params={encodedParams}";
        }
    }
}