using Reto.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Reto.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Se requiere un nombre de usuario")]
    public string? nombre {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere contraseña")]
    public string? contraseña {get; set;}
    private readonly DataBaseContext _context;

    [BindProperty]
    public string? messageNombre {get; set;}
    [BindProperty]
    public string? messageContra {get; set;}

    public IndexModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {

    }

    public void OnPostCrearCuenta() {
        Response.Redirect("CrearCuenta");
    }
    public void OnPostIniciarSesion() {
        bool nombreCorrecto = false, contraCorrecta = false;
        if (ModelState.IsValid) {
            if(nombre != null) {
                if (_context.verificarUsuario(nombre)) {
                    messageNombre = null;
                    HttpContext.Session.SetString("nombreSesion", nombre);
                    nombreCorrecto = true;
                }
                else {
                    messageNombre = "Usuario incorrecto";
                }
            }
            else {
                HttpContext.Session.SetString("nombreSesion", "SIN SESIÓN");
            }

            if (contraseña != null) {
                if (_context.verificarContra(nombre, contraseña) && nombreCorrecto) {
                    messageContra = null;
                    HttpContext.Session.SetString("contraSesion", contraseña);
                    contraCorrecta = true;
                }
                else {
                    messageContra = "Contrasena incorrecta";
                }
            }
            else {
                HttpContext.Session.SetString("contraSesion", "SIN SESIÓN");
            }
            if (contraCorrecta && nombreCorrecto) {
                Response.Redirect("Dashboard");
            }
        }
    }
}
