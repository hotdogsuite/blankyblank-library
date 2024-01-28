using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlankyBlankLibrary.Pages;

public class PasswordEditModel : PageModel {

    private readonly Services.PasswordServices _passwordServices;

    public PasswordEditModel (Services.PasswordServices passwordServices) => _passwordServices = passwordServices;

    [BindProperty]
    public ViewModels.PasswordEdit Password { get; set; } = null!;

    public void OnGet (int id) {
        Password = _passwordServices.GetPasswordEdit(id);
    }

    public async Task<IActionResult> OnPostSave () {
        int id = await _passwordServices.UpdatePassword(Password);
        return RedirectToPage(new { id });
    }

    public IActionResult OnPostAddAlternateSpelling () {
        Password.AlternateSpellings.Add(new ViewModels.PasswordEdit.AlternateSpelling());
        return Page();
    }

    public IActionResult OnPostAddForbiddenWord () {
        Password.ForbiddenWords.Add(new ViewModels.PasswordEdit.ForbiddenWord());
        return Page();
    }

    public IActionResult OnPostAddTailoredWord () {
        Password.TailoredWords.Add(new ViewModels.PasswordEdit.TailoredWord());
        return Page();
    }

}