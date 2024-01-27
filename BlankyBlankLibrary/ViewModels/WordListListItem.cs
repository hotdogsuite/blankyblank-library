namespace BlankyBlankLibrary.ViewModels;

public class WordListListItem {

        public int Id { get; set; }

        public string Name { get; set; } = null!;
        
        public string? Placeholder { get; set; }

        public int? Amount { get; set; }

        public int? MaxChoices { get; set; } = null!;

        public bool Optional { get; set; }

        public IEnumerable<string> Words { get; set; } = null!;
        
        public IEnumerable<string> Lists { get; set; } = null!;

}