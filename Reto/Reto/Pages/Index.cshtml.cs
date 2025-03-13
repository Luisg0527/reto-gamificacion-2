using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reto.Pages;

public class IndexModel : PageModel
{
    public int? empleadoId {get; set;}
    public string? contraseña {get; set;}

    public void OnGet()
    {

    }

    public void OnPostCrearCuenta() {
        Response.Redirect("CrearCuenta");
    }
    public void OnPostIniciarSesion() {
        Response.Redirect("Privacy");
    }
}
