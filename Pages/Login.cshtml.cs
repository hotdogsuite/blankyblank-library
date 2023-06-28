using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class LoginModel : PageModel {
    
    private readonly SignInManager<IdentityUser> _signInManager;
    public LoginModel(SignInManager<IdentityUser> signInManager) {
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = null!;

    public class InputModel {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }

    public async Task<IActionResult> OnPostAsync() {
        if (ModelState.IsValid) {
            var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, isPersistent: Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded) return RedirectToPage("Index");
            else ViewData["Message"] = "Failed to log in.";
        }
        return Page();
    }
    
}