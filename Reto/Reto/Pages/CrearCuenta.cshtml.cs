using Reto.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Reto.Pages;

public class CrearCuentaModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Se requiere un el ID del empleado")]
    public int? empleadoId {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere nombre de usuario")]
    public string? nombre {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere correo")]
    public string? correo {get; set;}

    [BindProperty]
    [Required(ErrorMessage = "Se requiere contraseña")]
    public string? contraseña {get; set;}

    private readonly DataBaseContext _context;

    public CrearCuentaModel(DataBaseContext context)
    {
        _context = context;
    }
    public void OnGet()
    {

    }

    public void OnPostIniciarSesion() {
        Response.Redirect("Index");
    }
    public void OnPostCCCrearCuenta() {
        bool empId = false, nom = false, corr = false, cont = false;
        if (ModelState.IsValid) {
            if(empleadoId != null) {
                empId = true;
                HttpContext.Session.SetInt32("empleadoIdSesion", (int)empleadoId);
            }
            else {
                HttpContext.Session.SetInt32("empleadoIdSesion", -1);
            }

            if(nombre != null) {
                nom = true;
                HttpContext.Session.SetString("nombreSesion", nombre);
            }
            else {
                HttpContext.Session.SetString("nombreSesion", "SIN SESIÓN");
            }

            if(correo != null) {
                corr = true;
                HttpContext.Session.SetString("correoSesion", correo);
            }
            else {
                HttpContext.Session.SetString("correoSesion", "SIN SESIÓN");
            }

            if (contraseña != null) {
                cont = true;
                HttpContext.Session.SetString("contraSesion", contraseña);
            }
            else {
                HttpContext.Session.SetString("contraSesion", "SIN SESIÓN");
            }
            if (empId && nom && corr && cont) {
                _context.crearUsuario((int)empleadoId, nombre, contraseña, correo);
            }
            Response.Redirect("Index");
        }
    }
}