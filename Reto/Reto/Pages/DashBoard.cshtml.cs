using Reto.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Web;

public class DashboardModel : PageModel
{
    private readonly DataBaseContext _context;

    [BindProperty(SupportsGet = true)]
    public DateTime SelectedDate { get; set; } = DateTime.Today;

    public MetricasDash metricas { get; set; } = new MetricasDash(0, 0, 0, 0, 0, 0, DateTime.MinValue);

    public int empleadoId { get; set; }
    public string LookerStudioUrl { get; set; }

    public DashboardModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        metricas = _context.getMetricas(SelectedDate);
        empleadoId = HttpContext.Session.GetInt32("empleadoIdSesion") ?? -1;
        Console.WriteLine(empleadoId);
        
        // Construct Looker Studio URL with correct format
        var baseUrl = "https://lookerstudio.google.com/embed/u/0/reporting/89864ac7-9e36-4c62-a5b7-e3e5cf5d1520/page/nliIF";
        var paramsJson = $"{{\"idusr\":{empleadoId}}}";
        var encodedParams = HttpUtility.UrlEncode(paramsJson);
        LookerStudioUrl = $"{baseUrl}?params={encodedParams}";
    }
}
