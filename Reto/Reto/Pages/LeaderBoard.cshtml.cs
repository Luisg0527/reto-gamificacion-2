using Reto.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardModel : PageModel
{
    private readonly DataBaseContext _context;

    public List<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public LeaderboardModel(DataBaseContext context)
    {
        _context = context;
    }

    public void OnGet()
    {
        Usuarios = _context.GetLeaderboard();
    }
}