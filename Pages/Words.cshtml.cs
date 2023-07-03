using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlankyBlankLibrary.Pages;

public class WordsModel : PageModel {

    private readonly Services.WordServices _ws;

    public WordsModel(Services.WordServices ws) {
        _ws = ws;
    }

    public IList<ViewModels.WordList> Words { get; set; } = null!;

    public void OnGet() {
        Words = _ws.GetWordList();
    }
    
}