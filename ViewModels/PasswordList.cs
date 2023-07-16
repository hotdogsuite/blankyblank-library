namespace BlankyBlankLibrary.ViewModels;

public class PasswordList {

    public int Id { get; set; }

    public int? ImportedId { get; set; }

    public string Password { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? Subcategory { get; set; }

    public string Difficulty { get; set; } = null!;

    public bool UsCentric { get; set; }

    public bool IncludeInExport { get; set; }

    public IList<string> AlternativeSpellings { get; set; } = null!;

    public IList<string> ForbiddenWords { get; set; } = null!;

}