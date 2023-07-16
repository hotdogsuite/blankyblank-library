using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class PasswordsModel : PageModel {
    
    private readonly Services.PasswordServices _passwordServices;

    public PasswordsModel(Services.PasswordServices passwordServices) {
        _passwordServices = passwordServices;
    }

    public IList<ViewModels.PasswordList> Passwords { get; set; } = null!;

    public void OnGet() {
        Passwords = _passwordServices.GetPasswords();
    }

    public async Task<IActionResult> OnPostToggleExport(int passwordId, bool includeInExport) {
        await _passwordServices.SetIncludeInExport(passwordId, includeInExport);
        return RedirectToPage("/Passwords");
    }

}