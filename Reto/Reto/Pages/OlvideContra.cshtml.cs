using Reto.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Reto.Pages;

public class OlvideContraModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Se requiere su nombre de usuario")]
    public string? usuario {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere una contraseña")]
    public string? contra {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere confirmar la contraseña")]
    public string? confirmaContra {get; set;}

    [BindProperty]
    public string? messageUsuario {get; set;}

    [BindProperty]
    public string? messageContra {get; set;}

    private readonly DataBaseContext _context;

    public OlvideContraModel(DataBaseContext context)
    {
        _context = context;
    }
    
    public void OnGet()
    {

    }

    public void OnPostReestablecer() {
        bool usCorrecto = false, samePass = false;
        if (ModelState.IsValid) {
            if(usuario != null) {
                if (_context.verificarUsuario(usuario)) {
                    messageUsuario = null;
                    usCorrecto = true;
                }
                else {
                    messageUsuario = "Usuario incorrecto";
                }
            }

            if (contra != null) {
            }
            else {
                HttpContext.Session.SetString("contraSesion", "SIN SESIÓN");
            }
            if(contra != confirmaContra) {
                messageContra = "Las contrasenas no son iguales";
            }
            else {
                samePass = true;
                messageContra = null;
            }
            
            if (confirmaContra != null) {
                HttpContext.Session.SetString("contraSesion", confirmaContra);
            }
            else {
                HttpContext.Session.SetString("contraSesion", "SIN SESIÓN");
            }
            if (usCorrecto && samePass) {
                _context.cambiarContra(usuario, confirmaContra);
                Response.Redirect("Index");
            }
            
        }
    }
}
