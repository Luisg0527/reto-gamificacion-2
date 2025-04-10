using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reto.Model;
using MySql.Data.MySqlClient;

namespace Reto.Pages;

public class PrivacyModel : PageModel
{
    public readonly DataBaseContext _context;
    public List<Pregunta> preguntas { get; set; } = new List<Pregunta>();
    public List<Respuesta> respuestas { get; set; } = new List<Respuesta>();
    public bool liked = false;
    public Dictionary<int, bool> PreguntasLikeadas { get; set; } = new();
    public Dictionary<int, bool> RespuestasLikeadas { get; set; } = new();

    public PrivacyModel(DataBaseContext context)
    {
        _context = context;
    }
    public void OnGet()
    {

        preguntas = _context.obtenerPregunta();

        foreach (var pregunta in preguntas)
        {
            bool yaLikeado = _context.UsuarioDioLikePregunta(HttpContext.Session.GetInt32("empleadoIdSesion") ?? 1, pregunta.id_pregunta);
            PreguntasLikeadas[pregunta.id_pregunta] = yaLikeado;
        }
        foreach (var respuesta in respuestas)
        {
            var like = HttpContext.Session.GetInt32($"liked_ans_{respuesta.id_respuesta}") == 1;
            RespuestasLikeadas[respuesta.id_respuesta] = like;
        }
    }

    public IActionResult OnPostIncrementLikePregunta(int id)
    {
        string sessionKey = $"liked_{id}";
        int? likedStatus = HttpContext.Session.GetInt32(sessionKey);

        if (likedStatus == null || likedStatus == 0)
        {
            // Usuario da like
            HttpContext.Session.SetInt32(sessionKey, 1);
            _context.agregarLikePregunta(id);
        }
        else
        {
            // Usuario quita like
            HttpContext.Session.SetInt32(sessionKey, 0);
            _context.quitarLikePregunta(id);
        }

        return RedirectToPage();
    }

    public IActionResult OnPostIncrementLikeRespuesta(int id)
    {
        // Si el id ya tiene un like registrado
        if (RespuestasLikeadas.ContainsKey(id))
        {
            // Invertimos el valor del like (de true a false o de false a true)
            RespuestasLikeadas[id] = !RespuestasLikeadas[id];
        }
        else
        {
            // Si no tiene like registrado, lo agregamos con el valor 'true' (likeada)
            RespuestasLikeadas.Add(id, true);
        }

        // Realizamos la operación según el estado actual del like
        if (RespuestasLikeadas[id])
        {
            // Si está likeada (true), quitamos el like
            _context.quitarLikeRespuesta(id);
        }
        else
        {
            // Si no está likeada (false), agregamos el like
            _context.agregarLikeRespuesta(id);
        }

        // Redirigimos de vuelta
        return RedirectToPage();
    }


    public IActionResult OnPostAgregarPregunta(string textoPregunta)
    {
        int? id;
        if (HttpContext.Session.GetInt32("empleadoIdSesion") == null)
        {
            id = 1;
        }
        else
        {
            id = HttpContext.Session.GetInt32("empleadoIdSesion");
        }
        _context.agregarPregunta(textoPregunta, id);
        return RedirectToPage();
    }

    public IActionResult OnPostAgregarRespuesta(string textoRespuesta, int idPregunta)
    {
        int? id;
        if (HttpContext.Session.GetInt32("empleadoIdSesion") == null)
        {
            id = 1;
        }
        else
        {
            id = HttpContext.Session.GetInt32("empleadoIdSesion");
        }
        _context.agregarRespuesta(textoRespuesta, id, idPregunta);
        return RedirectToPage();
    }

    public IActionResult OnPostLikePregunta(int idPregunta)
    {
        int id = HttpContext.Session.GetInt32("empleadoIdSesion") ?? 1;
        bool yaLikeado = _context.UsuarioDioLikePregunta(id, idPregunta);
        _context.likePregunta(id, idPregunta, yaLikeado);
        return RedirectToPage();
    }

    public IActionResult OnPostLikeRespuesta(int idRespuesta)
    {
        int id = HttpContext.Session.GetInt32("empleadoIdSesion") ?? 1;
        bool yaLikeado = _context.UsuarioDioLikeRespuesta(id, idRespuesta);
        _context.likeRespuesta(id, idRespuesta, yaLikeado);
        return RedirectToPage();
    }

}

