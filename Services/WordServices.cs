namespace BlankyBlankLibrary.Services;

public class WordServices {

    private readonly Data.AppDbContext _db;

    public WordServices(Data.AppDbContext db) {
        _db = db;
    }

    public IList<ViewModels.WordList> GetWordList() {
        return _db.Words.Select(x => new ViewModels.WordList() {
            Id = x.Id,
            Name = x.Name,
            Placeholder = x.Placeholder,
            MaxChoices = x.MaxChoices,
            Amount = x.Amount,
            Optional = x.Optional ? "Yes" : "No",
            Words = x.Words.Select(y => new ViewModels.WordList.WordListWord() {
                    Id = y.Id,
                    AlwaysChoose = y.AlwaysChoose,
                    Word = y.Word,
                    LinkedWords = y.LinkedWord != null ? y.LinkedWord.Words.Select(y => new ViewModels.WordList.WordListWord() {
                        AlwaysChoose = y.AlwaysChoose,
                        Word = y.Word
                    })
                    .ToList() : null
                })
                .ToList()
        })
        .ToList();
    }

}