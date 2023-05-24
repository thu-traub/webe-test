// =========================================================
//  Demo App, Lecture Webengineering, S. Traub 2023
//
//  Handle a single person
// =========================================================

using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace demo.Pages;

public class PersonModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public readonly IDataStore ds;
    public Person curr = new();

    public PersonModel(ILogger<IndexModel> logger, IDataStore ds)
    {
        _logger = logger;
        this.ds = ds;
    }

    public void OnGet(int Id)
    {
        curr = ds.getPerson(Id) ?? new();
    }

    public void OnPost(Person p, string save)
    {
        string ReName = "^[A-Za-zÖÄÜöäüß \\-]*$";
        string RePhone = "^[0-9+\\- ]*$";

        // Never ever trust input from web forms. Check for valie entries
        bool check = !String.IsNullOrEmpty(p.FirstName) &&
                     !String.IsNullOrEmpty(p.LastName) &&
                     !String.IsNullOrEmpty(p.Phone) &&
                     Regex.IsMatch(p.FirstName,ReName) && 
                     Regex.IsMatch(p.LastName,ReName) && 
                     Regex.IsMatch(p.Phone,RePhone);
                     
        if (check) {
            if (save == "save") {
                if (p.Id == 0) ds.createPerson(p); else ds.updatePerson(p); 
            }
            else if (save == "delete") ds.deletePerson(p);
            Response.Redirect("/");
        } else {
            curr = p;
        }
    }
}
