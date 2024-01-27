using System.Text;
using System.Text.RegularExpressions;
using BlankyBlankLibrary.Extensions;
using Newtonsoft.Json;

namespace BlankyBlankLibrary.Services;

public class PasswordServices {

    private readonly Data.AppDbContext _db;

    public PasswordServices (Data.AppDbContext db) => _db = db;

    public async Task ImportPasswords (IFormFile passwordsFile) {

        ViewModels.PasswordsContainer passwordsContainer = JsonConvert.DeserializeObject<ViewModels.PasswordsContainer>(await passwordsFile.ReadFileContents()) ?? throw new Exception("What're you feeding me?");

        var trackedDbPasswords = new Dictionary<string, Data.Models.Passwords.Password>();
        
        foreach (var password in passwordsContainer.Passwords.Where(pw => !string.IsNullOrEmpty(pw.Name))) {

            try {

                Data.Models.Passwords.Password? dbPassword;
                
                if (!trackedDbPasswords.TryGetValue(password.Name, out dbPassword)) {
                    dbPassword = _db.Passwords.SingleOrDefault(pw => pw.Name == password.Name);
                    if (dbPassword == null) {
                        dbPassword = new Data.Models.Passwords.Password() {
                            LegacyId = Convert.ToInt32(password.Id),
                            Name = password.Name.Trim(),
                            Difficulty = password.Difficulty,
                            Category = password.Category.Trim(),
                            Subcategory = string.IsNullOrEmpty(password.Subcategory.Trim()) ? null : password.Subcategory.Trim(),
                            AlternateSpellings = password.AlternateSpellings
                                                .Select(alternate_spelling => new Data.Models.Passwords.AlternateSpelling() {
                                                    Spelling = alternate_spelling
                                                }).ToList(),
                            ForbiddenWords = password.ForbiddenWords
                                                .Where(forbidden_word => !string.IsNullOrEmpty(forbidden_word))
                                                .Select(forbidden_word => new Data.Models.Passwords.ForbiddenWord() {
                                                    Word = forbidden_word
                                                }).ToList(),
                            TailoredWords = password.TailoredWords
                                                // This one list -- place-feel-emotional-simple -- is referenced a couple times but doesn't exist in the word lists
                                                .Where(tailored_word => tailored_word.List != "<place-feel-emotional-simple>")
                                                .Select(tailored_word => new Data.Models.Passwords.TailoredWord() {
                                                    List = _db.WordLists.Single(word_list => word_list.Name == tailored_word.List.Trim(new char[] { ' ', '<', '>' })), // ?? throw new Exception($"Couldn't find list for `{tailored_word.List}`."),
                                                    Word = tailored_word.Word
                                                }).ToList(),
                            UsCentric = password.Us
                        };
                        _db.Passwords.Add(dbPassword);
                    }
                    trackedDbPasswords.Add(password.Name, dbPassword);
                }

            } catch (Exception ex) {
                
                throw new Exception($"Error processing password `{password}`.", ex);

            }

        }

        await _db.SaveChangesAsync();

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ViewModels.FileContainer ExportPasswords () {

        ViewModels.PasswordsContainer container = new ViewModels.PasswordsContainer {
            Passwords = _db.Passwords
                .Select(password => new ViewModels.PasswordsContainer.Password() {
                    Id = password.Id.ToString(),
                    Name = password.Name,
                    Difficulty = password.Difficulty,
                    Category = password.Category,
                    Subcategory = password.Subcategory == null ? "" : password.Subcategory,
                    AlternateSpellings = password.AlternateSpellings.Select(alternate_spelling => alternate_spelling.Spelling),
                    ForbiddenWords = password.ForbiddenWords.Select(forbidden_word => forbidden_word.Word),
                    TailoredWords = password.TailoredWords.Select(tailored_word => new ViewModels.PasswordsContainer.Password.TailoredWord() {
                        List = $"<{tailored_word.List.Name}>",
                        Word = tailored_word.Word
                    }),
                    Us = password.UsCentric
                }).ToList()
        };

        var json = JsonConvert.SerializeObject(container);

        return new ViewModels.FileContainer() {
            FileContents = Encoding.UTF8.GetBytes(json),
            FileContentType = "application/json",
            FileName = "BlankyBlankPasswords.jet"
        };
    }
    
    public IEnumerable<ViewModels.PasswordListItem> GetPasswords () {
        return _db.Passwords
            .OrderBy(password => password.Name)
            .Select(password => new ViewModels.PasswordListItem() {
                Id = password.Id,
                Name = password.Name,
                Difficulty = password.Difficulty,
                Category = password.Category,
                Subcategory = password.Subcategory,
                UsCentric = password.UsCentric,
                AlternateSpellings = password.AlternateSpellings.Select(alternate_spelling => alternate_spelling.Spelling),
                ForbiddenWords = password.ForbiddenWords.Select(forbidden_word => forbidden_word.Word),
                TailoredWords = password.TailoredWords.Select(tailored_word => new ViewModels.PasswordListItem.TailoredWord() {
                    Word = tailored_word.Word,
                    ListName = tailored_word.List.Name
                })
            });
    }

}