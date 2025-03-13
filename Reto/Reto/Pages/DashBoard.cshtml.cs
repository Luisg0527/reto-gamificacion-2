using Reto.Model;
using Reto.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;


public class DashboardModel : PageModel
{

    private readonly DataBaseContext _context;
    public DateTime SelectedDate { get; set; } = DateTime.Today;

  
    public MetricasDash metricas {get;set;}

     public DashboardModel(DataBaseContext context){
        _context = context;
    }


    public void OnGet()
    {
      metricas = _context.getMetricas();

    }
}
