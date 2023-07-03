namespace BlankyBlankLibrary.ViewModels;

public class WordList {

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Placeholder { get; set; }

    public int? MaxChoices { get; set; }

    public int? Amount { get; set; }

    public string Optional { get; set; } = null!;

    public IList<WordListWord> Words { get; set; } = null!;

    public class WordListWord {

        public int Id { get; set; }

        public bool AlwaysChoose { get; set; }

        public string Word { get; set; } = null!;

        public IList<WordListWord>? LinkedWords { get; set; } = null!;

    }

}