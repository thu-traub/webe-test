using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace demo.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public readonly IDataStore ds;

    public IndexModel(ILogger<IndexModel> logger, IDataStore ds)
    {
        _logger = logger;
        this.ds = ds;
    }

    public void OnGet()
    {
    }

    public void OnPost()
    {
        Person p = new();
        ds.createPerson(p);
        Response.Redirect($"Person/{p.Id}");
    }
}
