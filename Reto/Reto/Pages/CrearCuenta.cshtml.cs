using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reto.Pages;

public class CrearCuentaModel : PageModel
{
    public int? empleadoId {get; set;}
    public string? correo {get; set;}
    public string? contraseña {get; set;}
    public void OnGet()
    {

    }

    public void OnPostIniciarSesion() {
        Response.Redirect("Index");
    }
    public void OnPostCCCrearCuenta() {
        Response.Redirect("Privacy");
    }
}