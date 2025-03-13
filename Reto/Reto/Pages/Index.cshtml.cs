using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Reto.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Se requiere un correo")]
    public string? correo {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere contraseña")]
    public string? contraseña {get; set;}

    public void OnGet()
    {

    }

    public void OnPostCrearCuenta() {
        Response.Redirect("CrearCuenta");
    }
    public void OnPostIniciarSesion() {
        if (ModelState.IsValid) {
            if(correo != null) {
                HttpContext.Session.SetString("correoSesion", correo);
            }
            else {
                HttpContext.Session.SetString("correoSesion", "SIN SESIÓN");
            }
            if (contraseña != null) {
                HttpContext.Session.SetString("contraSesion", contraseña);
            }
            else {
                HttpContext.Session.SetString("contraSesion", "SIN SESIÓN");
            }
            Response.Redirect("Privacy");
        }
    }
}
