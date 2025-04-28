using Reto.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

public class HomeModel : PageModel
{
    public Usuario myUser = new Usuario();

    // Conexion a base de datos
    private readonly DataBaseContext _context;
    public HomeModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        myUser = _context.GetUserByName(HttpContext.Session.GetString("nombreSesion"));
    }
}