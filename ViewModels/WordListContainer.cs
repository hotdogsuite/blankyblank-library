using Newtonsoft.Json;

namespace BlankyBlankLibrary.ViewModels;

public class WordListContainer {

    [JsonProperty("content")]
    public IEnumerable<WordList> WordLists { get; set; } = null!;

    public class WordList {

        [JsonProperty("amount")]
        public string? Amount { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("maxChoices")]
        public string MaxChoices { get; set; } = null!;

        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("optional")]
        public bool Optional { get; set; }

        [JsonProperty("placeholder")]
        public string Placeholder { get; set; } = null!;

        [JsonProperty("words")]
        public IEnumerable<Word> Words { get; set; } = null!;
        
        public class Word {

            [JsonProperty("alwaysChoose")]
            public bool AlwaysChoose { get; set; }

            [JsonProperty("word")]
            public string WordName { get; set; } = null!;

        }

    }

}