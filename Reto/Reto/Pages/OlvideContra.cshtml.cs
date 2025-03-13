using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Reto.Pages;

public class OlvideContraModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Se requiere una contraseña")]
    public string? contra {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere confirmar la contraseña")]
    public string? confirmaContra {get; set;}
    
    public void OnGet()
    {

    }

    public void OnPostReestablecer() {
        if (ModelState.IsValid) {
            if (contra != null) {
            }
            else {
                HttpContext.Session.SetString("contraSesion", "SIN SESIÓN");
            }
            if (confirmaContra != null) {
                HttpContext.Session.SetString("contraSesion", confirmaContra);
            }
            else {
                HttpContext.Session.SetString("contraSesion", "SIN SESIÓN");
            }
            Response.Redirect("Index");
        }
    }
}
