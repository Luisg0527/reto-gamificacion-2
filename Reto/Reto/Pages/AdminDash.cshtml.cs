using Reto.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AdminDashModel : PageModel
{
    public string LookerStudioUrl { get; set; }

    public void OnGet()
    {
        // Obtener el empleadoId
        var empleadoId = HttpContext.Session.GetInt32("empleadoIdSesion") ?? -1;
        
        // Construir el link de looker studio ya con los parametros
        var baseUrl = "https://lookerstudio.google.com/embed/reporting/0a97c541-2fe6-4c62-b4d2-a6bc38914589/page/jWbIF";
        var paramsJson = $"{{\"idgerente\":{empleadoId}}}";
        var encodedParams = HttpUtility.UrlEncode(paramsJson);
        LookerStudioUrl = $"{baseUrl}?params={encodedParams}";
    }
}