using System.ComponentModel.DataAnnotations;
using BlankyBlankLibrary.Data.Models.Passwords;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class PasswordsModel : PageModel {

    private readonly Services.PasswordServices _passwordServices;

    public PasswordsModel (Services.PasswordServices passwordServices) => _passwordServices = passwordServices;

    [BindProperty, Display(Name = "Secret Prompts")]
    public IFormFile IncomingSecretPrompts { get; set; } = null!;

    public IEnumerable<ViewModels.PasswordListItem> ExistingPasswords { get; set; } = null!;

    public void OnGet () {
        ExistingPasswords = _passwordServices.GetPasswords();
    }

    public async Task<IActionResult> OnPostImport () {
        await _passwordServices.ImportPasswords(IncomingSecretPrompts);
        return RedirectToPage();
    }

    public IActionResult OnPostExport () {
        var export = _passwordServices.ExportPasswords();
        return File(export.FileContents, export.FileContentType, export.FileName);
    }

}