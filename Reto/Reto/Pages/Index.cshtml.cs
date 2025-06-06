using Reto.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Reto.Pages;

public class IndexModel : PageModel
{
    // Nombre y contraseña con sus errores de mensaje en caso de que esten vacios
    [BindProperty]
    [Required(ErrorMessage = "Se requiere un nombre de usuario")]
    public string? nombre {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere contraseña")]
    public string? contraseña {get; set;}

    // Conexion a base de datos
    private readonly DataBaseContext _context;

    // Mensajes en caso de que la contraseña o usuario sean incorrectos
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
                    // Get and set the empleadoId for this user
                    var empleadoId = _context.getEmpleadoId(nombre);
                    if (empleadoId.HasValue) {
                        HttpContext.Session.SetInt32("empleadoIdSesion", empleadoId.Value);
                    }
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
                    messageContra = "Contraseña incorrecta";
                }
            }
            else {
                HttpContext.Session.SetString("contraSesion", "SIN SESIÓN");
            }

            if (contraCorrecta && nombreCorrecto) {
                HttpContext.Session.SetString("puestoUsuario", _context.getTipoEmpleado(nombre));

                if(HttpContext.Session.GetString("puestoUsuario") == "Gerente") {
                    Response.Redirect("AdminDash");
                }
                else {
                    Response.Redirect("Home");
                }
            }
        }
    }
}
