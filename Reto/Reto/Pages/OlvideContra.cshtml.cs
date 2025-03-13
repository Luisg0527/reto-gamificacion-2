using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Reto.Pages;

public class OlvideContraModel : PageModel
{
    public int? contra {get; set;}
    public string? confirmaContra {get; set;}
    
    public void OnGet()
    {

    }

    public void OnPostReestablecer() {
        Response.Redirect("Index");
    }
}
