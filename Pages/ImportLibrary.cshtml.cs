using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BlankyBlankLibrary.Pages;

public class ImportLibraryModel : PageModel {

    private readonly Data.AppDbContext _db;
    private readonly ILogger<ImportLibraryModel> _logger;

    public ImportLibraryModel(Data.AppDbContext db, ILogger<ImportLibraryModel> logger) {
        _db = db;
        _logger = logger;
    }

    [BindProperty, Display(Name = "Library path")]
    public string LibraryPath { get; set; } = "/Users/bill/repos/BlankyBlank Game Files/content";

    public string Notice { get; set; } = null!;

    public async Task OnPost() {
                
        string passwordsPath = Path.Combine(LibraryPath, "BlankyBlankPasswords.jet");

        string passwordsFolderPath = Path.Combine(LibraryPath, "BlankyBlankPasswords");

        JsonModels.PasswordsModel passwords = JsonConvert.DeserializeObject<JsonModels.PasswordsModel>(System.IO.File.ReadAllText(passwordsPath))!;

        List<JsonModels.FolderDataModel> passwordsFolders = new List<JsonModels.FolderDataModel>();
        foreach (string folder in Directory.GetDirectories(passwordsFolderPath)) {
            JsonModels.FolderDataModel jetModel = JsonConvert.DeserializeObject<JsonModels.FolderDataModel>(System.IO.File.ReadAllText(Path.Combine(folder, "data.jet")))!;
            passwordsFolders.Add(jetModel);
        }

        // Process passwords
        {
            string path = Path.Combine(LibraryPath, "BlankyBlankPasswords.jet");
            JsonModels.PasswordsModel jsonModel = JsonConvert.DeserializeObject<JsonModels.PasswordsModel>(System.IO.File.ReadAllText(passwordsPath))!;
            foreach (var item in jsonModel.Content) {
                var dbItem = new Data.Models.Password();

                dbItem.AlternateSpellings = item.AlternateSpellings.Select(x => new Data.Models.Password.AlternateSpelling() {
                    Value = Scrub(x) ?? throw new Exception("Value can not be empty.")
                }).ToList();

                dbItem.ForbiddenWords = item.ForbiddenWords
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => new Data.Models.Password.ForbiddenWord() {
                        Value = Scrub(x) ?? throw new Exception("Value can not be empty.")
                    }).ToList();

                dbItem.TailoredWords = item.TailoredWords.Select(x => new Data.Models.Password.TailoredWord() {
                    List = Scrub(x.List) ?? throw new Exception("Value can not be empty."),
                    Word = Scrub(x.Word) ?? throw new Exception("Value can not be empty.")
                }).ToList();

                dbItem.Category = string.IsNullOrWhiteSpace(item.Category) ? throw new Exception("Category can not be empty.") : item.Category.Trim();
                dbItem.Difficulty = string.IsNullOrWhiteSpace(item.Difficulty) ? throw new Exception("Difficulty can not be empty.") : item.Difficulty.Trim();
                dbItem.ImportedId = Convert.ToInt32(item.Id);
                dbItem.PasswordPassword = string.IsNullOrWhiteSpace(item.Password) ? throw new Exception("Password can not be empty.") : item.Password.Trim();
                dbItem.Subcategory = string.IsNullOrWhiteSpace(item.Subcategory) ? null : item.Subcategory.Trim();
                dbItem.UsCentric = item.Us;

                _db.Passwords.Add(dbItem);
            }
        }

        // Process stuctures
        {
            string path = Path.Combine(LibraryPath, "BlankyBlankSentenceStructures.jet");
            JsonModels.SentenceStructuresModel jsonModel = JsonConvert.DeserializeObject<JsonModels.SentenceStructuresModel>(System.IO.File.ReadAllText(path)) ?? throw new Exception("Something bad happened here.");
            foreach (var item in jsonModel.Content) {
                var dbStructure = new Data.Models.SentenceStructure();
                dbStructure.Category = string.IsNullOrWhiteSpace(item.Category) ? throw new Exception("Category can not be empty.") : item.Category;
                dbStructure.ImportedId = Convert.ToInt32(item.Id);
                dbStructure.Structures = item.Structures.Select(x => new Data.Models.SentenceStructure.StructureStructure() { Value = x }).ToList();
                _db.Structures.Add(dbStructure);
            }
        }

        // Process words
        {
            string wordListsPath = Path.Combine(LibraryPath, "BlankyBlankWordLists.jet");
            JsonModels.WordListsModel wordLists = JsonConvert.DeserializeObject<JsonModels.WordListsModel>(System.IO.File.ReadAllText(wordListsPath))!;

            var dbWords = new List<Data.Models.Word>();

            foreach (var word in wordLists.Content) {

                _logger.LogInformation($"Processing word w/ id {word.Id}");

                var dbWord = new Data.Models.Word();

                dbWord.Amount = string.IsNullOrWhiteSpace(word.Amount) ? null : Convert.ToInt32(word.Amount);
                dbWord.ImportedId = string.IsNullOrWhiteSpace(word.Id) ? throw new Exception("Incoming ID can not be empty.") : Convert.ToInt32(word.Id);
                dbWord.MaxChoices = string.IsNullOrWhiteSpace(word.MaxChoices) ? null : Convert.ToInt32(word.MaxChoices);
                dbWord.Name = Scrub(word.Name) ?? throw new Exception("Name can not be empty.");
                dbWord.Optional = word.Optional;
                dbWord.Placeholder = string.IsNullOrWhiteSpace(word.Placeholder) ? null : Scrub(word.Placeholder);
                dbWord.Words = word.Words.Select(x => new Data.Models.Word.WordWord() {
                    AlwaysChoose = x.AlwaysChoose,
                    Word = x.Word
                }).ToList();

                dbWords.Add(dbWord);

            }

            foreach (var word in dbWords) {
                foreach (var subWord in word.Words.Where(x => x.Word != "<gadget>")) {
                    var pattern = "^<([a-z-]+)>$";
                    if (Regex.IsMatch(subWord.Word, pattern)) {
                        try {
                            subWord.LinkedWord = dbWords.Single(x => x.Name == Regex.Replace(subWord.Word, pattern, "$1"));
                        } catch (Exception ex) {
                            throw new Exception($"Couldn't find `{Regex.Replace(subWord.Word, pattern, "$1")}`.", ex);
                        }
                    }
                }
                // foreach (var wordWord in word.Words) {
                //     var match = Regex.Match(wordWord.Word, "^<([a-z-]*)>$");
                //     if (match.Success) {
                //         var linkedWord = dbWords.SingleOrDefault(x => x.Name == match.Groups[1].Value);
                //         if (linkedWord != null) wordWord.LinkedWord = linkedWord;
                //         else _logger.LogInformation($"Couldn't find `{match.Groups[1].Value}`.");
                //     }
                // }
                _db.Words.Add(word);
            }

        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("*** DING! ***");

    }

    private string? Scrub(string input) {
        if (string.IsNullOrWhiteSpace(input)) {
            return null;
        }
        return input.Trim();
    }

    private bool AreArraysEqual(IList<string> list1, IList<string> list2) {
        if (list1.Count() != list2.Count()) return false;
        for (int i = 0; i < list1.Count(); i++) {
            if (list1[i] != list2[i]) return false;
        }
        return true;
    }

}