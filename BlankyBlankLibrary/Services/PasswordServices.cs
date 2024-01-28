using System.Text;
using System.Text.RegularExpressions;
using BlankyBlankLibrary.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlankyBlankLibrary.Services;

public class PasswordServices {

    private readonly Data.AppDbContext _db;

    public PasswordServices (Data.AppDbContext db) => _db = db;

    public async Task ImportPasswords (IFormFile passwordsFile) {

        ViewModels.PasswordsContainer passwordsContainer = JsonConvert.DeserializeObject<ViewModels.PasswordsContainer>(await passwordsFile.ReadFileContents()) ?? throw new Exception("What're you feeding me?");

        var trackedPasswords = new Dictionary<string, Data.Models.Passwords.Password>();
        var trackedCategories = new Dictionary<string, Data.Models.Passwords.Category>();
        var trackedSubcategories = new Dictionary<string, Data.Models.Passwords.Subcategory>();
        
        foreach (var password in passwordsContainer.Passwords.Where(pw => !string.IsNullOrEmpty(pw.Name))) {

            try {
                
                // Get category from incoming file
                Data.Models.Passwords.Category? dbCategory;

                if (!trackedCategories.TryGetValue(password.Category, out dbCategory)) {
                    dbCategory = _db.PasswordCategories.SingleOrDefault(cat => cat.CategoryName == password.Category);
                    if (dbCategory == null) dbCategory = new Data.Models.Passwords.Category() { CategoryName = password.Category };
                    trackedCategories.Add(password.Category, dbCategory);
                }

                // Get sub-category from incoming file
                Data.Models.Passwords.Subcategory? dbSubcategory;

                string? subcategoryName = string.IsNullOrEmpty(password.Subcategory) ? null : password.Subcategory;

                // Some adjustments from the incoming stock file, correcting typos and errant categories
                if (dbCategory.CategoryName == "person") {
                    if (subcategoryName == "celebrities") subcategoryName = "celebrity";
                    else if (subcategoryName == "game") subcategoryName = "fictional";
                    else if (subcategoryName == "leader") subcategoryName = "famous";
                    else if (subcategoryName == "musican") subcategoryName = "musician";
                    else if (subcategoryName == "watery") subcategoryName = "water";
                }
                else if (dbCategory.CategoryName == "story") {
                    if (subcategoryName == "boove") subcategoryName = "boovie";
                }

                string subcategoryKey = password.Category + ":" + (string.IsNullOrEmpty(subcategoryName) ? "" : subcategoryName);

                if (!trackedSubcategories.TryGetValue(subcategoryKey, out dbSubcategory)) {
                    dbSubcategory = _db.PasswordSubcategories.SingleOrDefault(cat => cat.SubcategoryName == subcategoryName && cat.Category.CategoryName == password.Category);
                    if (dbSubcategory == null) dbSubcategory = new Data.Models.Passwords.Subcategory() {
                        Category = dbCategory,
                        SubcategoryName = subcategoryName
                    };
                    trackedSubcategories.Add(subcategoryKey, dbSubcategory);
                }
                
                // Get password from incoming file
                Data.Models.Passwords.Password? dbPassword;
                
                if (!trackedPasswords.TryGetValue(password.Name, out dbPassword)) {
                    dbPassword = _db.Passwords.SingleOrDefault(pw => pw.Name == password.Name);
                    if (dbPassword == null) {
                        dbPassword = new Data.Models.Passwords.Password() {
                            LegacyId = Convert.ToInt32(password.Id),
                            IncludeInExport = true,
                            Name = password.Name.Trim(),
                            Difficulty = password.Difficulty,
                            Subcategory = dbSubcategory,
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
                    trackedPasswords.Add(password.Name, dbPassword);
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
                .Where(password => password.IncludeInExport)
                .Select(password => new ViewModels.PasswordsContainer.Password() {
                    Id = password.Id.ToString(),
                    Name = password.Name,
                    Difficulty = password.Difficulty,
                    Category = password.Subcategory.Category.CategoryName,
                    Subcategory = password.Subcategory.SubcategoryName == null ? "" : password.Subcategory.SubcategoryName,
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
                IncludeInExport = password.IncludeInExport,
                Difficulty = password.Difficulty,
                Category = password.Subcategory.Category.CategoryName,
                Subcategory = password.Subcategory.SubcategoryName,
                UsCentric = password.UsCentric,
                AlternateSpellings = password.AlternateSpellings.Select(alternate_spelling => alternate_spelling.Spelling),
                ForbiddenWords = password.ForbiddenWords.Select(forbidden_word => forbidden_word.Word),
                TailoredWords = password.TailoredWords.Select(tailored_word => new ViewModels.PasswordListItem.TailoredWord() {
                    Word = tailored_word.Word,
                    ListName = tailored_word.List.Name
                })
            });
    }

    public ViewModels.PasswordEdit GetPasswordEdit (int id = 0) {
        try {
            if (id == 0) return new ViewModels.PasswordEdit();
            return _db.Passwords
                .Where(password => password.Id == id)
                .Select(password => new ViewModels.PasswordEdit() {
                    Id = password.Id,
                    IncludeInExport = password.IncludeInExport,
                    Name = password.Name,
                    Difficulty = password.Difficulty,
                    SubcategoryId = password.Subcategory.Id,
                    UsCentric = password.UsCentric,
                    AlternateSpellings = password.AlternateSpellings.Select(alt_sp => new ViewModels.PasswordEdit.AlternateSpelling() {
                        Id = alt_sp.Id,
                        DeleteOnSave = false,
                        Spelling = alt_sp.Spelling
                    })
                    .ToList(),
                    ForbiddenWords = password.ForbiddenWords.Select(for_wd => new ViewModels.PasswordEdit.ForbiddenWord() {
                        Id = for_wd.Id,
                        DeleteOnSave = false,
                        Word = for_wd.Word
                    })
                    .ToList(),
                    TailoredWords = password.TailoredWords.Select(tlr_wd => new ViewModels.PasswordEdit.TailoredWord() {
                        Id = tlr_wd.Id,
                        DeleteOnSave = false,
                        Word = tlr_wd.Word,
                        ListId = tlr_wd.List.Id
                    })
                    .ToList()
                })
                .Single();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error loading password with id {id}.", ex);
        }

    }

    public IEnumerable<SelectListItem> GetCategories () {
        return _db.PasswordSubcategories
            .OrderBy(cat => cat.Category.CategoryName)
            .ThenBy(cat => cat.SubcategoryName)
            .Select(cat => new SelectListItem(cat.Category.CategoryName + " > " + (cat.SubcategoryName ?? "just " + cat.Category.CategoryName), cat.Id.ToString()));
    }

    public IEnumerable<SelectListItem> GetWordLists () {
        return _db.WordLists
            .Select(word_list => new SelectListItem(word_list.Name, word_list.Id.ToString()))
            .ToList()
            .OrderBy(item => item.Text);
    }

    public async Task<int> UpdatePassword (ViewModels.PasswordEdit passwordEdit) {

        Data.Models.Passwords.Password dbPassword;

        if (passwordEdit.Id == 0) {
            dbPassword = new Data.Models.Passwords.Password() {
                AlternateSpellings = new List<Data.Models.Passwords.AlternateSpelling>(),
                ForbiddenWords = new List<Data.Models.Passwords.ForbiddenWord>(),
                TailoredWords = new List<Data.Models.Passwords.TailoredWord>()
            };
            _db.Passwords.Add(dbPassword);
        } else {
            dbPassword = _db.Passwords
                .Include(pw => pw.AlternateSpellings)
                .Include(pw => pw.ForbiddenWords)
                .Include(pw => pw.TailoredWords)
                    .ThenInclude(tw => tw.List)
                .Single(pw => pw.Id == passwordEdit.Id);
        }

        // Basic Information
        dbPassword.IncludeInExport = passwordEdit.IncludeInExport;
        dbPassword.Name = passwordEdit.Name;
        dbPassword.Difficulty = passwordEdit.Difficulty;
        dbPassword.Subcategory = _db.PasswordSubcategories.Single(sub => sub.Id == passwordEdit.SubcategoryId);
        dbPassword.UsCentric = passwordEdit.UsCentric;
        
        // Alternate Spellings
        foreach (var altSpelling in passwordEdit.AlternateSpellings) {

            // Add new if id = 0 and not flagged for deletion
            if (altSpelling.Id == 0) {
                if (altSpelling.DeleteOnSave || string.IsNullOrWhiteSpace(altSpelling.Spelling)) { continue; }
                dbPassword.AlternateSpellings.Add(new Data.Models.Passwords.AlternateSpelling() { Spelling = altSpelling.Spelling });
            }
            else {
                if (altSpelling.DeleteOnSave) {
                    dbPassword.AlternateSpellings.Remove(dbPassword.AlternateSpellings.Single(alt => alt.Id == altSpelling.Id));
                }
                else {
                    dbPassword.AlternateSpellings.Single(alt => alt.Id == altSpelling.Id).Spelling = altSpelling.Spelling;
                }
            }
        }

        // Forbidden Words
        foreach (var forbiddenWord in passwordEdit.ForbiddenWords) {

            // Add new if id = 0 and not flagged for deletion
            if (forbiddenWord.Id == 0) {
                if (forbiddenWord.DeleteOnSave || string.IsNullOrWhiteSpace(forbiddenWord.Word)) { continue; }
                dbPassword.ForbiddenWords.Add(new Data.Models.Passwords.ForbiddenWord() { Word = forbiddenWord.Word });
            }
            else {
                if (forbiddenWord.DeleteOnSave) {
                    dbPassword.ForbiddenWords.Remove(dbPassword.ForbiddenWords.Single(alt => alt.Id == forbiddenWord.Id));
                }
                else {
                    dbPassword.ForbiddenWords.Single(alt => alt.Id == forbiddenWord.Id).Word = forbiddenWord.Word;
                }
            }
        }

        // Tailored Words
        foreach (var tailoredWord in passwordEdit.TailoredWords) {

            // Add new if id = 0 and not flagged for deletion
            if (tailoredWord.Id == 0) {
                if (tailoredWord.DeleteOnSave || string.IsNullOrWhiteSpace(tailoredWord.Word)) { continue; }
                dbPassword.TailoredWords.Add(new Data.Models.Passwords.TailoredWord() {
                    Word = tailoredWord.Word,
                    List = _db.WordLists.Single(wl => wl.Id == tailoredWord.ListId)
                });
            }
            else {
                if (tailoredWord.DeleteOnSave) {
                    dbPassword.TailoredWords.Remove(dbPassword.TailoredWords.Single(alt => alt.Id == tailoredWord.Id));
                }
                else {
                    var dbTailoredWord = dbPassword.TailoredWords.Single(alt => alt.Id == tailoredWord.Id);
                    dbTailoredWord.Word = tailoredWord.Word;
                    dbTailoredWord.List = _db.WordLists.Single(wl => wl.Id == tailoredWord.ListId);
                }
            }
        }

        await _db.SaveChangesAsync();
        return dbPassword.Id;
    }

}