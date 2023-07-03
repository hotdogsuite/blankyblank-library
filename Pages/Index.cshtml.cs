using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Data.AppDbContext _db;

    public IndexModel(ILogger<IndexModel> logger, Data.AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IList<ListView> Passwords { get; set; } = null!;

    public class ListView {
        public int Id { get; set; }
        public int? ImportedId { get; set; }
        public string Password { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string? Subcategory { get; set; }
        public string Difficulty { get; set; } = null!;
        public bool UsCentric { get; set; }
        public IList<string> AlternativeSpellings { get; set; } = null!;
        public IList<string> ForbiddenWords { get; set; } = null!;
    }

    public void OnGet()
    {
        Passwords = _db.Passwords.Select(x => new ListView() {
            Id = x.Id,
            ImportedId = x.ImportedId,
            Password = x.PasswordPassword,
            Category = x.Category,
            Subcategory = x.Subcategory,
            Difficulty = x.Difficulty,
            UsCentric = x.UsCentric,
            AlternativeSpellings = x.AlternateSpellings.Select(x => x.Value).ToList(),
            ForbiddenWords = x.ForbiddenWords.Select(x => x.Value).ToList()
        }).ToList();
    }
}
