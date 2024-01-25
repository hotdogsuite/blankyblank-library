using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class WordsModel : PageModel {

    private readonly Services.WordServices _wordServices;

    public WordsModel (Services.WordServices wordServices) => _wordServices = wordServices;

    [BindProperty, Display(Name = "Word List")]
    public IFormFile WordList { get; set; } = null!;

    public int WordListCount { get; set; }

    public void OnGet () {
        WordListCount = _wordServices.GetWordListCount();
    }

    public async Task<IActionResult> OnPostImport () {
        await _wordServices.ImportWordList(WordList);
        return RedirectToPage();
    }

    public IActionResult OnPostExport () {
        var export = _wordServices.ExportWordList();
        return File(export.FileContents, export.FileContentType, export.FileName);
    }

}