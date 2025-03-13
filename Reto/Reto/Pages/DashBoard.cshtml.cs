using Reto.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;

public class DashboardModel : PageModel
{
    private readonly DataBaseContext _context;

    [BindProperty(SupportsGet = true)]
    public DateTime SelectedDate { get; set; } = DateTime.Today;

    public MetricasDash metricas { get; set; } = new MetricasDash(0, 0, 0, 0, 0, 0, DateTime.MinValue);

    public DashboardModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        metricas = _context.getMetricas(SelectedDate);
    }
}
