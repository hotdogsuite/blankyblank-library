using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class WordsModel : PageModel {

    private readonly Services.WordServices _wordServices;

    public WordsModel (Services.WordServices wordServices) => _wordServices = wordServices;

    [BindProperty, Display(Name = "Word List")]
    public IFormFile WordList { get; set; } = null!;

    public async Task OnPostImport () {
        await _wordServices.ImportWordList(WordList);
    }

    public IActionResult OnPostExport () {
        var export = _wordServices.ExportLegacyWordList();
        return File(export.FileContents, export.FileContentType, export.FileName);
    }

}