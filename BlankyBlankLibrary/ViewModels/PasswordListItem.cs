namespace BlankyBlankLibrary.ViewModels;

public class PasswordListItem {

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Difficulty { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string? Subcategory { get; set; }

    public bool UsCentric { get; set; }

    public IEnumerable<string> AlternateSpellings { get; set; } = null!;

    public IEnumerable<string> ForbiddenWords { get; set; } = null!;

    public IEnumerable<TailoredWord> TailoredWords { get; set; } = null!;

    public class TailoredWord {

        public string Word { get; set; } = null!;

        public string ListName { get; set; } = null!;

    }
    
}