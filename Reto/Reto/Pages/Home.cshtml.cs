using Reto.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

public class HomeModel : PageModel
{
    public Usuario myUser = new Usuario();
    public MetricasDash metricas = new MetricasDash(0, 0, 0, 0, 0, 0, DateTime.MinValue);
    public int LugarEnLeaderboard { get; set; }
    public Pregunta UltimaPregunta { get; set; }

    // Conexion a base de datos
    private readonly DataBaseContext _context;
    public HomeModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        myUser = _context.GetUserByName(HttpContext.Session.GetString("nombreSesion"));
        metricas = _context.getMetricas(DateTime.Today);
        
        // Get leaderboard position
        var leaderboard = _context.GetLeaderboard();
        LugarEnLeaderboard = leaderboard.FindIndex(u => u.nombre_usuario == myUser.nombre_usuario) + 1;

        // Get latest forum post
        var preguntas = _context.obtenerPregunta();
        UltimaPregunta = preguntas.OrderByDescending(p => p.date).FirstOrDefault();
    }
}