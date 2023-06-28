namespace BlankyBlank.Models;

public class PasswordDataModel {
    public string Password { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? Subcategory { get; set; } = null!;
    public string Difficulty { get; set; } = null!;
    public string[]? ForbiddenWords { get; set; } = new string[0];
    public string[]? AlternateSpellings { get; set; } = new string[0];
}