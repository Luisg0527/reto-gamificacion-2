using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reto.Model;
using Microsoft.AspNetCore.Http;

namespace Reto.Pages
{
    public class UsuarioModel : PageModel
    {
        private readonly DataBaseContext _dbContext;

        [BindProperty]
        public Usuario UsuarioActual { get; set; }

        [BindProperty]
        public int LugarEnLeaderboard { get; set; }

        // Esta propiedad controla el modo de edición (si se muestran los campos de editar)
        [BindProperty]
        public bool EditMode { get; set; }

        public UsuarioModel()
        {
            _dbContext = new DataBaseContext();
        }

        public void OnGet()
        {
            // Obtenemos el nombre del usuario de la sesión; en este ejemplo se usa "jPerez" por defecto
            string nombreSesion = HttpContext.Session.GetString("nombreSesion") ?? "jPerez"; 
            UsuarioActual = _dbContext.GetUsuarioByNombre(nombreSesion);
            
            // Obtener el lugar en el leaderboard
            var leaderboard = _dbContext.GetLeaderboard();
            LugarEnLeaderboard = leaderboard.FindIndex(u => u.nombre_usuario == nombreSesion) + 1;
        }

        // Alterna el modo de edición
        public IActionResult OnPostToggleEdit()
        {
            // Preserve the current user data
            string nombreSesion = HttpContext.Session.GetString("nombreSesion") ?? "jPerez";
            UsuarioActual = _dbContext.GetUsuarioByNombre(nombreSesion);
            
            EditMode = !EditMode;
            return Page();
        }

        // Al guardar se actualizan los datos en la base de datos y se desactiva el modo edición
        public IActionResult OnPostGuardar()
        {
            if (UsuarioActual == null)
            {
                return Page();
            }

            // Actualiza la BD con los nuevos datos
            _dbContext.ActualizarUsuario(UsuarioActual);
            EditMode = false;
            return RedirectToPage();
        }
    }
}