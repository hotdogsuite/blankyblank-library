using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class WordsModel : PageModel {

    private readonly Services.WordListServices _wordServices;

    public WordsModel (Services.WordListServices wordServices) => _wordServices = wordServices;

    [BindProperty, Display(Name = "Word Lists")]
    public IFormFile IncomingWordList { get; set; } = null!;

    public IEnumerable<ViewModels.WordListListItem> ExistingWordLists { get; set; } = null!;

    public int WordListCount { get; set; }

    public void OnGet () {
        ExistingWordLists = _wordServices.GetWordLists();
        WordListCount = _wordServices.GetWordListCount();
    }

    public async Task<IActionResult> OnPostImport () {
        await _wordServices.ImportWordList(IncomingWordList);
        return RedirectToPage();
    }

    public IActionResult OnPostExport () {
        var export = _wordServices.ExportWordList();
        return File(export.FileContents, export.FileContentType, export.FileName);
    }

}