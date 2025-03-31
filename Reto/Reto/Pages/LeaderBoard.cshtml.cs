using Reto.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System;

public class LeaderboardModel : PageModel
{
    private readonly DataBaseContext _context;

    public LeaderboardModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
    }
}
