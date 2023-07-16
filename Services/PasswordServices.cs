namespace BlankyBlankLibrary.Services;

public class PasswordServices {

    private readonly Data.AppDbContext _db;

    public PasswordServices(Data.AppDbContext db) {
        _db = db;
    }

    public IList<ViewModels.PasswordList> GetPasswords() {
        return _db.Passwords.Select(x => new ViewModels.PasswordList() {
            Id = x.Id,
            ImportedId = x.ImportedId,
            Password = x.PasswordPassword,
            Category = x.Category.Category,
            Subcategory = x.Subcategory,
            Difficulty = x.Difficulty,
            UsCentric = x.UsCentric,
            IncludeInExport = x.IncludeInExport,
            AlternativeSpellings = x.AlternateSpellings.Select(y => y.Value).ToList(),
            ForbiddenWords = x.ForbiddenWords.Select(y => y.Value).ToList()
        })
        .ToList();
    }

    public async Task SetIncludeInExport(int passwordId, bool includeInExport) {
        var password = _db.Passwords.Where(x => x.Id == passwordId).Single();
        password.IncludeInExport = includeInExport;
        await _db.SaveChangesAsync();
    }
}