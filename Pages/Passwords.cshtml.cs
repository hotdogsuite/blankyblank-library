using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class PasswordsModel : PageModel {
    
    private readonly Data.AppDbContext _db;

    public PasswordsModel(Data.AppDbContext db) {
        _db = db;
    }

    public IList<ViewModels.PasswordList> Passwords { get; set; } = null!;

    public void OnGet() {
        Passwords = _db.Passwords.OrderBy(x => x.ImportedId)
            .Select(x => new ViewModels.PasswordList() {
                Password = x.PasswordPassword
            })
            .ToList();
    }

}